﻿
namespace ReheeCmf.Tenants
{
  public class TenantEntity : EntityBase<Guid>
  {
    public int? TenantLevel { get; set; }
    public string? TenantName { get; set; }
    public string? TenantSubDomain { get; set; }
    public string? NomolizationTenantSubDomain { get; set; }
    public DateTime? LicenceEnd { get; set; }

    public string? MainConnectionString { get; set; }
    public string? ReadConnectionStrings { get; set; }
    [NotMapped]
    public string[]? ReadConnectionStringArray { get; set; }

    public EnumFileService? FileServiceType { get; set; }
    public bool? FileCompression { get; set; }
    public string? FileCompressionFileExtensions { get; set; }
    public string? FileBaseFolder { get; set; }
    public string? FileServerPath { get; set; }
    public string? FileAccessToken { get; set; }
    public long? FileMaxFileUploadSize { get; set; }
    public string? FileAllowedRoles { get; set; }
    public bool? FileAuthRequired { get; set; }
    public string? FileAllowedFileType { get; set; }
  }

  [EntityChangeTracker<TenantEntity>(Index = -1, Group = nameof(TenantEntityHandler))]
  public class TenantEntityHandler : EntityChangeHandler<TenantEntity>
  {
    private ITenantStorage? storage { get; set; }
    public override void Init(IServiceProvider sp, object entity, int index, int subindex, string? group = null)
    {
      base.Init(sp, entity, index, subindex, group);
      storage = sp.GetService<ITenantStorage>();
    }
    public override void Dispose()
    {
      base.Dispose();
      storage = null;
    }
    public override Task SetTenant(CancellationToken ct = default)
    {
      entity!.TenantID = null;
      return Task.CompletedTask;
    }
    public override Task AfterCreateAsync(CancellationToken ct = default)
    {
      base.AfterCreateAsync(ct);
      storage!.AddOrUpdateTenant(entity!);
      return Task.CompletedTask;
    }
    public override Task AfterUpdateAsync(CancellationToken ct = default)
    {
      base.AfterUpdateAsync(ct);
      storage!.AddOrUpdateTenant(entity!);
      return Task.CompletedTask;
    }
    public override Task AfterDeleteAsync(CancellationToken ct = default)
    {
      base.AfterDeleteAsync(ct);
      storage!.AddOrUpdateTenant(entity!);
      return Task.CompletedTask;
    }
    public override async Task<IEnumerable<ValidationResult>> ValidationAsync(CancellationToken ct = default)
    {
      var baseValidation = (await base.ValidationAsync(ct)).Concat(getBasicValidation(entity!, context!));
      var tenantValidation = sp.GetService<ITenantEntityValidation>();

      if (tenantValidation != null)
      {
        var validation = await tenantValidation.ValidateAsync(entity!, context!, ct);
        var result = baseValidation.Concat(validation);
        return result;
      }
      return baseValidation;
    }
    private IEnumerable<ValidationResult> getBasicValidation(TenantEntity entity, IContext context)
    {
      if (String.IsNullOrWhiteSpace(entity.TenantSubDomain) || Common.EmptyString.Any(b => entity.TenantSubDomain.Contains(b)))
      {
        yield return ValidationResultHelper.New("Tenant domain cant with empty string", nameof(TenantEntity.TenantSubDomain));
      }
      entity!.NomolizationTenantSubDomain = entity!.TenantSubDomain?.ToUpper() ?? "";
      if (context!.Query<TenantEntity>(true)
        .Where(b => !b.Id.Equals(entity.Id) && b.NomolizationTenantSubDomain == entity.NomolizationTenantSubDomain)
        .Select(b => b.Id).Any())
      {
        yield return ValidationResultHelper.New("Tenant Duplicate", nameof(TenantEntity.TenantSubDomain));
      }
    }

  }
}