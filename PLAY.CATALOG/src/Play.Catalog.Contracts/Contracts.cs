using System;

namespace DefaultNamespace;

public record CatalogItemCreated(Guid Id, string Name, string Description);

public record CatalogItemUpdated(Guid Id, string Name, string Description);

public record CatalogItemDeleted(Guid Id);