using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OVB.Demos.Eschody.Domain.TenantContext.DataTransferObject;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Mappings;

public sealed class TenantMapping : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        #region Table Configuration

        builder.ToTable("tenants");

        #endregion

        #region Primary Key Configuration

        builder.HasKey(p => p.ClientId)
            .HasName("pk_tenant_client_id");

        #endregion

        #region Foreign Key Configuration



        #endregion

        #region Index Key Configuration

        builder.HasIndex(p => p.Cnpj)
            .IsUnique(true)
            .HasDatabaseName("uk_tenant_cnpj");

        #endregion

        #region Property Configuration

        builder.Property(p => p.ClientId)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnName("client_id")
            .HasMaxLength(Guid.NewGuid().ToString().Length)
            .HasColumnType("UUID")
            .ValueGeneratedNever();
        builder.Property(p => p.ClientSecret)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnName("client_secret")
            .HasMaxLength(Guid.NewGuid().ToString().Length)
            .HasColumnType("UUID")
            .ValueGeneratedNever();

        builder.Property(p => p.CorrelationId)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnName("correlation_id")
            .HasMaxLength(Guid.NewGuid().ToString().Length)
            .HasColumnType("UUID")
            .ValueGeneratedNever();
        builder.Property(p => p.SourcePlatform)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasColumnName("source_platform")
            .HasMaxLength(AuditableInfoValueObject.SourcePlatformMaxLength)
            .ValueGeneratedNever();
        builder.Property(p => p.ExecutionUser)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasColumnName("execution_user")
            .HasMaxLength(AuditableInfoValueObject.ExecutionUserMaxLength)
            .ValueGeneratedNever();
        builder.Property(p => p.CreatedAt)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnType("TIMESTAMPTZ")
            .HasColumnName("created_at")
            .ValueGeneratedNever();

        builder.Property(p => p.ComercialName)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasColumnName("comercial_name")
            .HasMaxLength(255)
            .ValueGeneratedNever();
        builder.Property(p => p.SocialReason)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasColumnName("social_reason")
            .HasMaxLength(255)
            .ValueGeneratedNever();
        builder.Property(p => p.Cnpj)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnType("CHAR")
            .HasColumnName("document_cnpj")
            .HasMaxLength(14)
            .ValueGeneratedNever();
        builder.Property(p => p.Composition)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("SMALLINT")
            .HasColumnName("composition")
            .ValueGeneratedNever();
        builder.Property(p => p.PrimaryCnaeCode)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnType("CHAR")
            .HasColumnName("primary_cnae_code")
            .HasMaxLength(7)
            .ValueGeneratedNever();
        builder.Property(p => p.Scope)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasColumnName("scope")
            .HasMaxLength(255)
            .ValueGeneratedNever();
        builder.Property(p => p.IsTenantAvailableUntil)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("TIMESTAMPTZ")
            .HasColumnName("is_available_until")
            .ValueGeneratedNever();
        builder.Property(p => p.IsTenantEnabled)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("BOOLEAN")
            .HasColumnName("is_tenant_enabled")
            .ValueGeneratedNever();

        builder.Property(p => p.LastCorrelationId)
           .IsRequired(true)
           .IsFixedLength(true)
           .HasColumnName("last_correlation_id")
           .HasMaxLength(Guid.NewGuid().ToString().Length)
           .HasColumnType("UUID")
           .ValueGeneratedNever();
        builder.Property(p => p.LastSourcePlatform)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasColumnName("last_source_platform")
            .HasMaxLength(AuditableInfoValueObject.SourcePlatformMaxLength)
            .ValueGeneratedNever();
        builder.Property(p => p.LastExecutionUser)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasColumnName("last_execution_user")
            .HasMaxLength(AuditableInfoValueObject.ExecutionUserMaxLength)
            .ValueGeneratedNever();
        builder.Property(p => p.LastModifiedAt)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnType("TIMESTAMPTZ")
            .HasColumnName("last_modified_at")
            .ValueGeneratedNever();

        #endregion
    }
}
