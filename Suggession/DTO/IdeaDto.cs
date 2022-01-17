using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Suggession.DTO
{

    public class IdeaDto
    {
        public int Id { get; set; }
        public string Sequence { get; set; }
        public int Index { get; set; }
        public int IdeaId { get; set; }
        public int? ReceiveID { get; set; }
        public int? SendID { get; set; }
        public string File_Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Issue { get; set; }
        public string Type { get; set; }
        public string Suggession { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string Comment { get; set; }
        public string CommentZh { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public bool Isshow { get; set; }
        public bool IsAnnouncement { get; set; }
        public int? ModifiedBy { get; set; }
        public List<Files> File { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
    public class Files
    {
        public string Path { get; set; }
    }
}
