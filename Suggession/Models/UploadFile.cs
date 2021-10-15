using Suggession.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.Models
{
    [Table("UploadFile")]
    public class UploadFile : IDateTracking
    {
        public UploadFile()
        {
        }

        public UploadFile(string path, int kPIId, DateTime uploadTime)
        {
            Path = path;
            IdealID = kPIId;
            UploadTime = uploadTime;
        }

        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
        public int IdealID { get; set; }
        public DateTime UploadTime { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
