namespace NeuroMedia.Domain.Enums
{
    [Flags]
    public enum Roles
    {
        None = 0,
        Patient = 1,
        Nurse = 2,
        Doctor = 4,
        Admin = 8,
        SystemManager = 16,
        MedicalGroup = Nurse | Doctor | Admin,
        InstitutionGroup = MedicalGroup | Patient,
        UserManagementGroup = Admin | SystemManager,
        All = Patient | SystemManager | InstitutionGroup
    }
}
