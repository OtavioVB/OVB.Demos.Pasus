using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OVB.Demos.Eschody.Domain.StudentContext;
using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Mappings;

public sealed class StudentMapping : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        #region Table Configuration

        builder.ToTable("students");

        #endregion

        #region Primary Key Configuration

        builder.HasKey(p => p.Id)
            .HasName("pk_student_id");

        #endregion

        #region Foreign Key Configuration



        #endregion

        #region Index Key Configuration

        builder.HasIndex(p => p.Email)
            .IsUnique(true)
            .HasDatabaseName("uk_student_email");

        #endregion

        #region Property Configuration

        builder.Property(p => p.Id)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnName("idstudent")
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

        builder.Property(p => p.FirstName)
            .IsRequired(true)
            .IsFixedLength(false)
            .HasColumnType("VARCHAR")
            .HasColumnName("first_name")
            .HasMaxLength(FirstNameValueObject.FirstNameMaxLength)
            .ValueGeneratedNever();
        builder.Property(p => p.LastName)
           .IsRequired(true)
           .IsFixedLength(false)
           .HasColumnType("VARCHAR")
           .HasColumnName("last_name")
           .HasMaxLength(LastNameValueObject.LastNameMaxLength)
           .ValueGeneratedNever();
        builder.Property(p => p.Email)
           .IsRequired(true)
           .IsFixedLength(false)
           .HasColumnType("VARCHAR")
           .HasColumnName("email")
           .HasMaxLength(EmailValueObject.EmailMaxLength)
           .ValueGeneratedNever();
        builder.Property(p => p.Phone)
           .IsRequired(true)
           .IsFixedLength(true)
           .HasColumnType("CHAR")
           .HasColumnName("phone")
           .HasMaxLength(PhoneValueObject.PhoneLength)
           .ValueGeneratedNever();
        builder.Property(p => p.Password)
            .IsRequired(true)
            .IsFixedLength(true)
            .HasColumnType("CHAR")
            .HasColumnName("password")
            .HasMaxLength(PasswordValueObject.PasswordEncryptLength)
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
