using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AutoMapperTest
{
    // [Serializable]
    public class ProductBasicModifyModelCopy
    {

        #region 双计量

        /// <summary> 辅计量单位 </summary>
        
        public int AuxMeasurementUnit { get; set; }

        /// <summary> 主辅比例 </summary>
        
        public int MainAuxRatio { get; set; }

        /// <summary> 是否称重 </summary>
        
        public bool IsWeigh { get; set; }


        #endregion


        #region 描述

        /// <summary> 商品加工类型描述 </summary>
        
        [Description("商品加工类型描述")]
        public string ProcessiTypeRemark { get; set; }

        ///// <summary> 辅计量单位 描述 </summary>
        //
        //public string AuxMeasurementUnitRemark { get; set; }

        ///// <summary> 主计量单位 描述 </summary>
        //
        //public string MainMeasurementUnitRemark { get; set; }


        #endregion


        #region

        /// <summary> 商品加工类型 </summary>
        
        [Description("商品加工类型")]
        public int ProcessiType { get; set; }

        /// <summary> 条形码 </summary>
        
        [Description("条形码")]
        public string BarCode { get; set; }

        /// <summary> 店内码 </summary>
        
        [Description("店内码")]
        public string InternalBarCode { get; set; }

        /// <summary> 箱规 </summary>
        
        [Description("箱规")]
        public int BoxSpecifications { get; set; }

        /// <summary> 大类 </summary>
        
        [Description("大类")]
        public int C1SysNo { get; set; }

        /// <summary> 中类 </summary>
        
        [Description("中类")]
        public int C2SysNo { get; set; }

        /// <summary> 小类 </summary>
        
        [Description("小类")]
        public int C3SysNo { get; set; }

        /// <summary> 创建时间 </summary>
        
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary> 创建人 </summary>
        
        [Description("创建人")]
        public int CreateUserSysNo { get; set; }

        /// <summary> 默认采购价格 </summary>
        
        [Description("默认采购价格")]
        public decimal DefaultPurchasePrice { get; set; }

        /// <summary> 默认供应商 </summary>
        
        [Description("默认供应商")]
        public int DefaultVendorSysNo { get; set; }

        /// <summary> 配送方式 </summary>
        
        [Description("配送方式")]
        public int DeliveryParttern { get; set; }

        /// <summary> 宅配配送次数 </summary>
        
        [Description("宅配配送次数")]
        public int EasyHomeDeliveryTimes { get; set; }

        /// <summary> 高 </summary>
        
        [Description("高")]
        public decimal HIGH { get; set; }

        /// <summary> 是否可以采购 </summary>
        
        [Description("是否可以采购")]
        public int IsCanPurchase { get; set; }

        /// <summary> 是否删除 </summary>
        
        [Description("是否删除")]
        public bool IsDelete { get; set; }

        /// <summary> 是否宅配 </summary>
        
        [Description("是否宅配")]
        public int IsEasyHome { get; set; }

        /// <summary> 是否是礼品卡 </summary>
        
        [Description("是否是礼品卡")]
        public int IsGiftCard { get; set; }

        /// <summary> 是否需加工 </summary>
        
        [Description("是否需加工")]
        public int IsNeedProcessing { get; set; }

        /// <summary> 是否删除 </summary>
        
        [Description("是否删除")]
        public bool IsOutOfSpecification { get; set; }

        /// <summary> 是否原产地 </summary>
        
        [Description("是否原产地")]
        public int IsProductOrigin { get; set; }

        /// <summary> 是否贵重 </summary>
        
        [Description("是否贵重")]
        public int IsValuable { get; set; }

        /// <summary> 最后修改时间 </summary>
        
        [Description("最后修改时间")]
        public DateTime LastUpdateTime { get; set; }

        /// <summary> 最后修改人 </summary>
        
        [Description("最后修改人")]
        public int LastUpdateUserSysNo { get; set; }

        /// <summary> 长 </summary>
        
        [Description("长")]
        public decimal Long { get; set; }

        /// <summary> 品牌 </summary>
        
        [Description("品牌")]
        public int ManufacturerSysNo { get; set; }

        /// <summary> 入驻商家 </summary>
        
        [Description("入驻商家")]
        public int MerchantSysNo { get; set; }

        /// <summary> 备注 </summary>
        
        [Description("备注")]
        public string Note { get; set; }

        /// <summary> 包装清单 </summary>
        
        [Description("包装清单")]
        public string PackageList { get; set; }

        /// <summary> 包装数量 </summary>
        
        [Description("包装数量")]
        public int PackQuantity { get; set; }

        /// <summary> 管理者 </summary>
        
        [Description("管理者")]
        public int PMUserSysNo { get; set; }

        /// <summary> 商品属性 </summary>
        
        [Description("商品属性")]
        public int ProductAttr { get; set; }

        /// <summary> 商品ID </summary>
        
        [Description("商品ID")]
        public string ProductID { get; set; }

        /// <summary> 商品规格 </summary>
        
        [Description("商品规格")]
        public string ProductMode { get; set; }

        /// <summary> 商品名称 </summary>
        
        [Description("商品名称")]
        public string ProductName { get; set; }

        /// <summary> 保质期提醒 </summary>
        
        [Description("保质期提醒")]
        public string ProductStorageDay { get; set; }

        /// <summary> 商品类型 </summary>
        
        [Description("商品类型")]
        public int ProductType { get; set; }

        /// <summary> 访问次数 </summary>
        
        [Description("访问次数")]
        public int ReviewCount { get; set; }

        /// <summary> 销项税 </summary>
        
        [Description("销项税")]
        public decimal SaleTaxRate { get; set; }

        /// <summary> 销售单位/主计量单位 </summary>
        
        [Description("销售单位/主计量单位")]
        public string SaleUnit { get; set; }

        /// <summary> 保质期 </summary>
        
        [Description("保质期")]
        public int ShelfLife { get; set; }

        /// <summary> 审核状态 </summary>
        
        [Description("审核状态")]
        public int Status { get; set; }

        /// <summary> 货架期 </summary>
        
        [Description("货架期")]
        public int StorageDay { get; set; }

        /// <summary> 存储方式 </summary>
        
        [Description("存储方式")]
        public int StorageMethods { get; set; }

        /// <summary> 编号 </summary>
        
        [Description("编号")]
        public int SysNo { get; set; }

        /// <summary> 进项税 </summary>
        
        [Description("进项税")]
        public decimal TaxRate { get; set; }

        /// <summary> 体积 </summary>
        
        [Description("体积")]
        public decimal Volume { get; set; }

        /// <summary> 重量 </summary>
        
        [Description("重量")]
        public int Weight { get; set; }

        /// <summary> 宽 </summary>
        
        [Description("宽")]
        public decimal Width { get; set; }


        /// <summary> 温馨提示权重 </summary>
        
        [Description("温馨提示权重")]
        public int WarmHintWeight { get; set; }

        /// <summary> 温馨提示 </summary>
        
        [Description("温馨提示")]
        public string WarmHint { get; set; }

        /// <summary>
        /// 获取或设置 TaxCode 属性值
        /// </summary>
        
        [Description("税务编码")]
        public string TaxCode { get; set; }

        /// <summary>
        /// 获取或设置 TaxC3SysNo 属性值
        /// </summary>
        
        [Description("税务类目")]
        public int TaxC3SysNo { get; set; }

        /// <summary>
        /// 获取或设置 WarningDay 属性值
        /// </summary>
        
        [Description("预警期")]
        public int WarningDay { get; set; }

        /// <summary> 产地 </summary>
        
        [Description("产地")]
        public string ProducingArea { get; set; }
        /// <summary> 产地SysNo </summary>
        
        [Description("产地SysNo")]
        public int ProductAreaSysNo { get; set; }

        /// <summary> 原料产地 </summary>
        
        [Description("原料产地")]
        public string OriginArea { get; set; }

        /// <summary> 原料产地编号 </summary>
        
        [Description("原料产地编号")]
        public int OriginAreaSysNo { get; set; }

        #endregion

        private List<string> updateParams = new List<string>();

        /// <summary>
        /// 获取或设置 UpdateParams 属性值 (需更新字段)
        /// </summary>
        
        public List<string> UpdateParams
        {
            get { return updateParams; }
            set { updateParams = value; }
        }


        /// <summary>商品编号 </summary>
        
        [Description("商品编号")]
        public int ProductSysNo { get; set; }

        /// <summary>克隆商品编号 </summary>
        
        [Description("克隆商品编号")]
        public int CloneProductSysNo { get; set; }

        /// <summary>申请人编号 </summary>
        
        [Description("申请人编号")]
        public int ApplicantSysNo { get; set; }

        /// <summary>申请时间 </summary>
        
        [Description("申请时间")]
        public DateTime ApplicantTime { get; set; }

        /// <summary>审核人编号 </summary>
        
        [Description("审核人编号")]
        public int AuditUserSysNo { get; set; }

        /// <summary>审核时间 </summary>
        
        [Description("审核时间")]
        public DateTime AuditTime { get; set; }

        /// <summary>审核状态 </summary>
        
        [Description("审核状态")]
        public int AuditStatus { get; set; }

        /// <summary>申请类型 </summary>
        
        [Description("申请类型")]
        public int ApplyType { get; set; }

        /// <summary>当前审核权限 </summary>
        
        [Description("当前审核权限")]
        public int AuditPrivilege { get; set; }

        /// <summary>
        /// 是否直采
        /// </summary>
        
        [Description("是否直采")]
        public bool IsDirectPurchase { get; set; }

    }
}
