using Falcon.MtG.Models.Sql;

namespace Falcon.API.DTO
{
    public class SetDto
    {
        public SetDto(Set s)
        {
            this.ID = s.ID;
            this.Name = s.Name;
            this.Code = s.Code;
            this.KeyruneCode = s.KeyruneCode;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string KeyruneCode { get; set; }
    }
}