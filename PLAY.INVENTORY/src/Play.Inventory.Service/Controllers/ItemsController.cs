using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers;

[Route("[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly IRepository<InventoryItem> _itemsRepository;
    private readonly CatalogClient _catalogClient;

    public ItemsController(IRepository<InventoryItem> itemsRepository, CatalogClient catalogClient)
    {
        _itemsRepository = itemsRepository;
        _catalogClient = catalogClient;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
    {
        if (userId == Guid.Empty) return BadRequest();

        var catalogItems = await _catalogClient.GetCatalogItemsAsync();
        var inventoryItems = await _itemsRepository.GetAllAsync(item => item.UserId == userId);

        var inventoryItemDtos = inventoryItems.Select(inventoryItem =>
        {
            var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
            return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
        });
        
        return Ok(inventoryItemDtos);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
    {
        var inventoryItem = await _itemsRepository.GetAsync(
            item => item.UserId == grantItemsDto.UserId &&
                    item.CatalogItemId == grantItemsDto.CatalogItemId);

        if (inventoryItem == null)
        {
            inventoryItem = new InventoryItem
            {
                CatalogItemId = grantItemsDto.CatalogItemId,
                UserId = grantItemsDto.UserId,
                Quantity = grantItemsDto.Quantity,
                AcquiredDate = DateTimeOffset.Now,
            };

            await _itemsRepository.CreateAsync(inventoryItem);
        }
        else
        {
            inventoryItem.Quantity += grantItemsDto.Quantity;
            await _itemsRepository.UpdateAsync(inventoryItem);
        }

        return Ok();
    }
}