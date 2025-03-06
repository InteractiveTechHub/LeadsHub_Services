using Ardalis.SmartEnum;

namespace InteractiveLeads.Core.Enums
{
    public class RolesEnum : SmartEnum<RolesEnum>
    {
        public static readonly RolesEnum SysAdmin = new(nameof(SysAdmin), 1);
        public static readonly RolesEnum Support = new(nameof(Support), 2);
        public static readonly RolesEnum Owner = new(nameof(Owner), 3);
        public static readonly RolesEnum Manager = new(nameof(Manager), 4);
        public static readonly RolesEnum Consultant = new(nameof(Consultant), 5);

        public RolesEnum(string name, int value) : base(name, value)
        {
        }
    }
}
