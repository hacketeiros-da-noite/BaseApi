using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlankApiModel.Model.Entities
{
    /// <summary>
    /// Example of entity model contains n of <see cref="JumpModel"/>
    /// </summary>
    [Table("giveyourjumps")]
    public class GiveYourJumpsModel
    {
        /// <summary>
        /// Key must be name id
        /// </summary>
        [Key]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// Used to ignore this in querys
        /// </summary>
        [NotMapped]
        public IEnumerable<JumpModel> Jumps { get; set; }
    }
}
