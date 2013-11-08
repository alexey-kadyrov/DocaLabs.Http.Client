namespace DocaLabs.Http.Client.Configuration
{
    public enum CertificateX509FindType
    {
        FindByThumbprint,
        FindBySubjectName,
        FindBySubjectDistinguishedName,
        FindByIssuerName,
        FindByIssuerDistinguishedName,
        FindBySerialNumber,
        FindByTimeValid,
        FindByTimeNotYetValid,
        FindByTimeExpired,
        FindByTemplateName,
        FindByApplicationPolicy,
        FindByCertificatePolicy,
        FindByExtension,
        FindByKeyUsage,
        FindBySubjectKeyIdentifier,
    }
}
