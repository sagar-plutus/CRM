using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class PersonShareData
    {
        Int32 id;
        String role;
        Int32 roleId;
        Int32 userId;
        Int32 personId;
        String name;
        Int32 pid;
        String docBase64;
        Int32 visitId;
        DateTime visitDate;
        String fileName;
        String emailId;
        int entityTypeId;

        public int Id { get => id; set => id = value; }
        public string Role { get => role; set => role = value; }
        public int RoleId { get => roleId; set => roleId = value; }
        public int UserId { get => userId; set => userId = value; }
        public int PersonId { get => personId; set => personId = value; }
        public string Name { get => name; set => name = value; }
        public int Pid { get => pid; set => pid = value; }
        public string DocBase64 { get => docBase64; set => docBase64 = value; }
        public int VisitId { get => visitId; set => visitId = value; }
        public DateTime VisitDate { get => visitDate; set => visitDate = value; }
        public string FileName { get => fileName; set => fileName = value; }
        public string EmailId { get => emailId; set => emailId = value; }
        public int EntityTypeId { get => entityTypeId; set => entityTypeId = value; }
    }
}
