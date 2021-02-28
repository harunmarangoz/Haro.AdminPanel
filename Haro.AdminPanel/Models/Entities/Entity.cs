using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Haro.AdminPanel.Models.Enums;

namespace Haro.AdminPanel.Models.Entities
{
    public class Entity : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public interface IEntity
    {
        long Id { get; set; }
    }
    public class Entry
    {
        public long Id { get; set; }
        public Language Language { get; set; }
        public long LanguageId { get; set; }
    }
}