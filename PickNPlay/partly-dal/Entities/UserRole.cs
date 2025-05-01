using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PickNPlay.picknplay_dal.Entities
{
    [Table("user_roles")]
    public partial class UserRole
    {
        public UserRole()
        {
            Users = new HashSet<User>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_role_id")]
        public int UserRoleId { get; set; }
        [Column("user_role_name")]
        [StringLength(50)]
        public string UserRoleName { get; set; } = null!;

        [InverseProperty(nameof(User.UserRole))]
        public virtual ICollection<User> Users { get; set; }
    }
}
