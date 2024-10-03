using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity.Core.Model;

namespace Web.Admin.Models
{
    public class ParentRoleViewModel
    {
        public ParentRoleViewModel() {
            Children = new List<RoleViewModel>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<RoleViewModel> Children { get; set; }


    }

    public class RoleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentName { get; set; }
        public Guid ParentId { get; set; }
    }



    public static partial class MapperHelper
    {
        public static void FromModel(this ParentRoleViewModel obj, AppRole model)
        {
            obj.Id = model.Id;
            obj.Name = model.Name;
            obj.Description = model.Description;

        }

        public static void FromModel(this RoleViewModel obj, AppRole model)
        {
            obj.Id = model.Id;
            obj.Name = model.Name;
            obj.Description = model.Description;
            obj.ParentName = model.ParentName;
            obj.ParentId = model.ParentId;

        }
    }
}