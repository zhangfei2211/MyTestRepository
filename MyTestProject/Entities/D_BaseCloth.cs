//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class D_BaseCloth
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> BaseClothFactoryId { get; set; }
        public Nullable<System.Guid> ClothId { get; set; }
        public string Width { get; set; }
        public Nullable<int> GramWeight { get; set; }
        public Nullable<int> Number { get; set; }
        public Nullable<System.Guid> ColorId { get; set; }
        public Nullable<decimal> TotalWeight { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public string remarks { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
