using OVB.Demos.Eschody.Domain.ValueObjects;
using OVB.Demos.Eschody.Libraries.NotificationContext;

namespace OVB.Demos.Eschody.Domain.Notifications;

public static class NotificationFacilitator
{
    public static Notification FirstNameMaxLength = Notification.BuildErrorfullNotification(
        code: "PASUS01",
        message: $"O nome precisa conter até {FirstNameValueObject.FirstNameMinLength} caracteres.");
    public static Notification FirstNameMinLength = Notification.BuildErrorfullNotification(
        code: "PASUS02",
        message: $"O nome precisa conter pelo menos {FirstNameValueObject.FirstNameMinLength} caracteres.");
    public static Notification LastNameMaxLength = Notification.BuildErrorfullNotification(
        code: "PASUS03",
        message: $"O sobrenome precisa conter até {LastNameValueObject.LastNameMaxLength} caracteres.");
    public static Notification LastNameMinLength = Notification.BuildErrorfullNotification(
        code: "PASUS04",
        message: $"O sobrenome precisa conter pelo menos {LastNameValueObject.LastNameMinLength} caracteres.");
    public static Notification EmailMaxLength = Notification.BuildErrorfullNotification(
        code: "PASUS05",
        message: $"O email precisa conter até {EmailValueObject.EmailMaxLength} caracteres.");
    public static Notification EmailMinLength = Notification.BuildErrorfullNotification(
        code: "PASUS06",
        message: $"O email precisa conter pelo menos {EmailValueObject.EmailMinLength} caracteres.");
    public static Notification EmailValid = Notification.BuildErrorfullNotification(
        code: "PASUS07",
        message: $"O email inserido precisa ser válido.");
    public static Notification PhoneLength = Notification.BuildErrorfullNotification(
        code: "PASUS08",
        message: $"O telefone precisa conter até {PhoneValueObject.PhoneLength} dígitos.");
    public static Notification PhoneDigit = Notification.BuildErrorfullNotification(
        code: "PASUS09",
        message: $"O telefone deve conter apenas dígitos.");
    public static Notification PasswordMaxLength = Notification.BuildErrorfullNotification(
        code: "PASUS10",
        message: $"A senha precisa conter até {PasswordValueObject.PasswordMaxLength} caracteres.");
    public static Notification PasswordMinLength = Notification.BuildErrorfullNotification(
        code: "PASUS11",
        message: $"A senha precisa conter pelo menos {PasswordValueObject.PasswordMinLength} caracteres.");
    public static Notification StudentExistDomain(int? index = null)
        => Notification.BuildErrorfullNotification(
        code: "PASUS12",
        message: "Esse estudante já existe com as credenciais enviadas.",
        index: index);
    public static Notification CredentialsNeedsToBeValid = Notification.BuildErrorfullNotification(
        code: "PASUS13",
        message: $"As credenciais da aplicação precisam ser válidas.");
    public static Notification CompositionNeedsToBeValid = Notification.BuildErrorfullNotification(
        code: "PASUS14",
        message: $"O tipo de composição da empresa deve ser válida.");
    public static Notification CnpjLength = Notification.BuildErrorfullNotification(
        code: "PASUS15",
        message: $"O CNPJ precisa conter {CnpjValueObject.CnpjLength} caracteres.");
    public static Notification CnpjDigit = Notification.BuildErrorfullNotification(
        code: "PASUS16",
        message: $"O CNPJ precisa conter apenas dígitos.");
    public static Notification SocialReasonMaxLength = Notification.BuildErrorfullNotification(
        code: "PASUS17",
        message: $"A razão social deve conter até {SocialReasonValueObject.MaxLength} caracteres.");
    public static Notification SocialReasonMinLength = Notification.BuildErrorfullNotification(
        code: "PASUS18",
        message: $"A razão social deve conter até {SocialReasonValueObject.MinLength} caracteres.");
    public static Notification ComercialNameMaxLength = Notification.BuildErrorfullNotification(
        code: "PASUS19",
        message: $"O nome comercial deve conter até {ComercialNameValueObject.MaxLength} caracteres.");
    public static Notification ComercialNameMinLength = Notification.BuildErrorfullNotification(
        code: "PASUS20",
        message: $"O nome comercial deve conter até {ComercialNameValueObject.MinLength} caracteres.");
    public static Notification CnaeCodeLength = Notification.BuildErrorfullNotification(
        code: "PASUS21",
        message: $"O código nacional de atividade econônomica (CNAE) deve conter até {CnaeCodeValueObject.Length} dígitos.");
    public static Notification CnaeCodeDigit = Notification.BuildErrorfullNotification(
        code: "PASUS22",
        message: $"O código nacional de atividade econônomica (CNAE) deve conter apenas dígitos.");
    public static Notification ScopeValid = Notification.BuildErrorfullNotification(
       code: "PASUS23",
       message: $"O escopo da aplicação cliente precisa ser válido.");
    public static Notification FoundationDateLessThanActualTime = Notification.BuildErrorfullNotification(
       code: "PASUS24",
       message: $"A data de fundação da empresa precisa ser igual ou menor a data atual.");
    public static Notification TenantExistsDomain = Notification.BuildErrorfullNotification(
       code: "PASUS25",
       message: $"O CNPJ utilizado pela corporativa já possui cadastro.");
    public static Notification GrantTypeValid = Notification.BuildErrorfullNotification(
       code: "PASUS26",
       message: $"O tipo de concessão de autorização precisa ser um tipo válido associado.");
    public static Notification TenantNotExists = Notification.BuildErrorfullNotification(
       code: "PASUS27",
       message: $"Não foi possível encontrar a aplicação cliente pelo seu identificador.");
    public static Notification TenantClientSecretInvalid = Notification.BuildErrorfullNotification(
       code: "PASUS28",
       message: $"A chave secreta inserida para a aplicação cliente não é válida.");
    public static Notification TenantIsNotAvailable(DateTime isAvailableUntil) => Notification.BuildErrorfullNotification(
       code: "PASUS29",
       message: $"A aplicação cliente não está apta para ser autorizada. Última disponibilidade: {DateTime.SpecifyKind(isAvailableUntil, DateTimeKind.Unspecified).AddHours(-3).ToString("dd/MM/yyyy HH:mm:ss")}.");
    public static Notification TenantIsEnabled = Notification.BuildErrorfullNotification(
       code: "PASUS30",
       message: $"A aplicação cliente não está habilitada nesse momento.");
    public static Notification TenantScopeIsNotValid(string scope) => Notification.BuildErrorfullNotification(
       code: "PASUS31",
       message: $"A aplicação cliente não está apta para utilizar os escopos de autorização enviado ({scope}).");

}
