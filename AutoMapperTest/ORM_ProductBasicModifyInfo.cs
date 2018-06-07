using System;
using System.ComponentModel;
using System.Data;
using Benlai.Common;

namespace AutoMapperTest
{
    public class ORM_ProductBasicModifyInfo : ModelProvider
    {
        #region 定义公共属性

        private int m_SysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取 SysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "SysNo", dataType: DbType.Int32, isPrimaryKey: true, isIdentity: true,
            isAllowNull: false, needInsert: false, needUpdate: false, defaultValue: AppConstCommon.IntNull)]
        
        [Description("编号")]
        public int SysNo
        {
            get { return m_SysNo; }
            set { m_SysNo = value; }
        }

        private string m_ProductID = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 ProductID 属性值
        /// </summary>
        [DataAttribute(fieldName: "ProductID", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("商品ID")]
        public string ProductID
        {
            get { return m_ProductID; }
            set { m_ProductID = value; }
        }

        private string m_ProductName = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 ProductName 属性值
        /// </summary>
        [DataAttribute(fieldName: "ProductName", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("商品名称")]
        public string ProductName
        {
            get { return m_ProductName; }
            set { m_ProductName = value; }
        }

        private string m_ProductMode = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 ProductMode 属性值
        /// </summary>
        [DataAttribute(fieldName: "ProductMode", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("商品规格")]
        public string ProductMode
        {
            get { return m_ProductMode; }
            set { m_ProductMode = value; }
        }

        private int m_ProductType = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 ProductType 属性值
        /// </summary>
        [DataAttribute(fieldName: "ProductType", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: false, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("商品类型")]
        public int ProductType
        {
            get { return m_ProductType; }
            set { m_ProductType = value; }
        }

        private int m_Weight = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 Weight 属性值
        /// </summary>
        [DataAttribute(fieldName: "Weight", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("重量")]
        public int Weight
        {
            get { return m_Weight; }
            set { m_Weight = value; }
        }

        private int m_ManufacturerSysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 ManufacturerSysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "ManufacturerSysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("品牌")]
        public int ManufacturerSysNo
        {
            get { return m_ManufacturerSysNo; }
            set { m_ManufacturerSysNo = value; }
        }

        private int m_DefaultVendorSysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 DefaultVendorSysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "DefaultVendorSysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("默认供应商")]
        public int DefaultVendorSysNo
        {
            get { return m_DefaultVendorSysNo; }
            set { m_DefaultVendorSysNo = value; }
        }

        private decimal m_DefaultPurchasePrice = AppConstCommon.DecimalNull;
        /// <summary>
        /// 获取或设置 DefaultPurchasePrice 属性值
        /// </summary>
        [DataAttribute(fieldName: "DefaultPurchasePrice", dataType: DbType.Decimal, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("默认采购价格")]
        public decimal DefaultPurchasePrice
        {
            get { return m_DefaultPurchasePrice; }
            set { m_DefaultPurchasePrice = value; }
        }

        private int m_IsCanPurchase = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 IsCanPurchase 属性值
        /// </summary>
        [DataAttribute(fieldName: "IsCanPurchase", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("是否可以采购")]
        public int IsCanPurchase
        {
            get { return m_IsCanPurchase; }
            set { m_IsCanPurchase = value; }
        }

        private string m_SaleUnit = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 SaleUnit 属性值
        /// </summary>
        [DataAttribute(fieldName: "SaleUnit", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("销售单位")]
        public string SaleUnit
        {
            get { return m_SaleUnit; }
            set { m_SaleUnit = value; }
        }

        private int m_StorageDay = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 StorageDay 属性值
        /// </summary>
        [DataAttribute(fieldName: "StorageDay", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("货架期")]
        public int StorageDay
        {
            get { return m_StorageDay; }
            set { m_StorageDay = value; }
        }

        private decimal m_Volume = AppConstCommon.DecimalNull;
        /// <summary>
        /// 获取或设置 Volume 属性值
        /// </summary>
        [DataAttribute(fieldName: "Volume", dataType: DbType.Decimal, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("体积")]
        public decimal Volume
        {
            get { return m_Volume; }
            set { m_Volume = value; }
        }

        private decimal m_Long = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 Long 属性值
        /// </summary>
        [DataAttribute(fieldName: "Long", dataType: DbType.Decimal, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("长")]
        public decimal Long
        {
            get { return m_Long; }
            set { m_Long = value; }
        }

        private decimal m_Width = AppConstCommon.DecimalNull;
        /// <summary>
        /// 获取或设置 Width 属性值
        /// </summary>
        [DataAttribute(fieldName: "Width", dataType: DbType.Decimal, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("宽")]
        public decimal Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        private decimal m_HIGH = AppConstCommon.DecimalNull;
        /// <summary>
        /// 获取或设置 HIGH 属性值
        /// </summary>
        [DataAttribute(fieldName: "HIGH", dataType: DbType.Decimal, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("高")]
        public decimal HIGH
        {
            get { return m_HIGH; }
            set { m_HIGH = value; }
        }

        private int m_IsGiftCard = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 IsGiftCard 属性值
        /// </summary>
        [DataAttribute(fieldName: "IsGiftCard", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("是否是礼品卡")]
        public int IsGiftCard
        {
            get { return m_IsGiftCard; }
            set { m_IsGiftCard = value; }
        }

        private int m_StorageMethods = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 StorageMethods 属性值
        /// </summary>
        [DataAttribute(fieldName: "StorageMethods", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("存储方式")]
        public int StorageMethods
        {
            get { return m_StorageMethods; }
            set { m_StorageMethods = value; }
        }

        private int m_BoxSpecifications = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 BoxSpecifications 属性值
        /// </summary>
        [DataAttribute(fieldName: "BoxSpecifications", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("箱规")]
        public int BoxSpecifications
        {
            get { return m_BoxSpecifications; }
            set { m_BoxSpecifications = value; }
        }

        private int m_ShelfLife = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 ShelfLife 属性值
        /// </summary>
        [DataAttribute(fieldName: "ShelfLife", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: false, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("保质期")]
        public int ShelfLife
        {
            get { return m_ShelfLife; }
            set { m_ShelfLife = value; }
        }

        private int m_IsEasyHome = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 IsEasyHome 属性值
        /// </summary>
        [DataAttribute(fieldName: "IsEasyHome", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("是否宅配")]
        public int IsEasyHome
        {
            get { return m_IsEasyHome; }
            set { m_IsEasyHome = value; }
        }

        private int m_EasyHomeDeliveryTimes = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 EasyHomeDeliveryTimes 属性值
        /// </summary>
        [DataAttribute(fieldName: "EasyHomeDeliveryTimes", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("宅配配送次数")]
        public int EasyHomeDeliveryTimes
        {
            get { return m_EasyHomeDeliveryTimes; }
            set { m_EasyHomeDeliveryTimes = value; }
        }

        private int m_IsNeedProcessing = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 IsNeedProcessing 属性值
        /// </summary>
        [DataAttribute(fieldName: "IsNeedProcessing", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("是否需加工")]
        public int IsNeedProcessing
        {
            get { return m_IsNeedProcessing; }
            set { m_IsNeedProcessing = value; }
        }

        private int m_ProcessiType = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 ProcessiType 属性值
        /// </summary>
        [DataAttribute(fieldName: "ProcessiType", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("加工类型")]
        public int ProcessiType
        {
            get { return m_ProcessiType; }
            set { m_ProcessiType = value; }
        }

        private int m_ProductAttr = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 ProductAttr 属性值
        /// </summary>
        [DataAttribute(fieldName: "ProductAttr", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("商品属性")]
        public int ProductAttr
        {
            get { return m_ProductAttr; }
            set { m_ProductAttr = value; }
        }

        private int m_C1SysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 C1SysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "C1SysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("大类")]
        public int C1SysNo
        {
            get { return m_C1SysNo; }
            set { m_C1SysNo = value; }
        }

        private int m_C2SysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 C2SysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "C2SysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("中类")]
        public int C2SysNo
        {
            get { return m_C2SysNo; }
            set { m_C2SysNo = value; }
        }

        private int m_C3SysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 C3SysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "C3SysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: false, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("小类")]
        public int C3SysNo
        {
            get { return m_C3SysNo; }
            set { m_C3SysNo = value; }
        }

        private int m_PMUserSysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 PMUserSysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "PMUserSysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("管理者")]
        public int PMUserSysNo
        {
            get { return m_PMUserSysNo; }
            set { m_PMUserSysNo = value; }
        }

        private int m_Status = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 Status 属性值
        /// </summary>
        [DataAttribute(fieldName: "Status", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("审核状态")]
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }

        private int m_LastUpdateUserSysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 LastUpdateUserSysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "LastUpdateUserSysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("最后修改人")]
        public int LastUpdateUserSysNo
        {
            get { return m_LastUpdateUserSysNo; }
            set { m_LastUpdateUserSysNo = value; }
        }

        private DateTime m_LastUpdateTime = AppConstCommon.DateTimeNull;
        /// <summary>
        /// 获取或设置 LastUpdateTime 属性值
        /// </summary>
        [DataAttribute(fieldName: "LastUpdateTime", dataType: DbType.DateTime, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: null)]
        
        [Description("最后修改时间")]
        public DateTime LastUpdateTime
        {
            get { return m_LastUpdateTime; }
            set { m_LastUpdateTime = value; }
        }

        private int m_CreateUserSysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 CreateUserSysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "CreateUserSysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("创建人")]
        public int CreateUserSysNo
        {
            get { return m_CreateUserSysNo; }
            set { m_CreateUserSysNo = value; }
        }

        private DateTime m_CreateTime = AppConstCommon.DateTimeNull;
        /// <summary>
        /// 获取或设置 CreateTime 属性值
        /// </summary>
        [DataAttribute(fieldName: "CreateTime", dataType: DbType.DateTime, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: null)]
        
        [Description("创建时间")]
        public DateTime CreateTime
        {
            get { return m_CreateTime; }
            set { m_CreateTime = value; }
        }

        private string m_PackageList = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 PackageList 属性值
        /// </summary>
        [DataAttribute(fieldName: "PackageList", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("包装清单")]
        public string PackageList
        {
            get { return m_PackageList; }
            set { m_PackageList = value; }
        }

        private int m_IsValuable = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 IsValuable 属性值
        /// </summary>
        [DataAttribute(fieldName: "IsValuable", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("是否贵重")]
        public int IsValuable
        {
            get { return m_IsValuable; }
            set { m_IsValuable = value; }
        }

        private string m_Note = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 Note 属性值
        /// </summary>
        [DataAttribute(fieldName: "Note", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("备注")]
        public string Note
        {
            get { return m_Note; }
            set { m_Note = value; }
        }

        private bool m_IsDelete = false;
        /// <summary>
        /// 获取或设置 IsDelete 属性值
        /// </summary>
        [DataAttribute(fieldName: "IsDelete", dataType: DbType.Boolean, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: false)]
        
        [Description("是否删除")]
        public bool IsDelete
        {
            get { return m_IsDelete; }
            set { m_IsDelete = value; }
        }

        private bool m_IsOutOfSpecification = false;
        /// <summary>
        /// 获取或设置 IsOutOfSpecification 属性值
        /// </summary>
        [DataAttribute(fieldName: "IsOutOfSpecification", dataType: DbType.Boolean, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: false)]
        
        [Description("是否删除")]
        public bool IsOutOfSpecification
        {
            get { return m_IsOutOfSpecification; }
            set { m_IsOutOfSpecification = value; }
        }

        private int m_MerchantSysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 MerchantSysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "MerchantSysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("入驻商家")]
        public int MerchantSysNo
        {
            get { return m_MerchantSysNo; }
            set { m_MerchantSysNo = value; }
        }

        private int m_IsProductOrigin = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 IsProductOrigin 属性值
        /// </summary>
        [DataAttribute(fieldName: "IsProductOrigin", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("是否原产地")]
        public int IsProductOrigin
        {
            get { return m_IsProductOrigin; }
            set { m_IsProductOrigin = value; }
        }

        private decimal m_SaleTaxRate = AppConstCommon.DecimalNull;
        /// <summary>
        /// 获取或设置 SaleTaxRate 属性值
        /// </summary>
        [DataAttribute(fieldName: "SaleTaxRate", dataType: DbType.Decimal, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("销项税")]
        public decimal SaleTaxRate
        {
            get { return m_SaleTaxRate; }
            set { m_SaleTaxRate = value; }
        }

        private decimal m_TaxRate = AppConstCommon.DecimalNull;
        /// <summary>
        /// 获取或设置 TaxRate 属性值
        /// </summary>
        [DataAttribute(fieldName: "TaxRate", dataType: DbType.Decimal, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("进项税")]
        public decimal TaxRate
        {
            get { return m_TaxRate; }
            set { m_TaxRate = value; }
        }

        private int m_PackQuantity = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 PackQuantity 属性值
        /// </summary>
        [DataAttribute(fieldName: "PackQuantity", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("包装数量")]
        public int PackQuantity
        {
            get { return m_PackQuantity; }
            set { m_PackQuantity = value; }
        }

        private int m_ReviewCount = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 ReviewCount 属性值
        /// </summary>
        [DataAttribute(fieldName: "ReviewCount", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("访问次数")]
        public int ReviewCount
        {
            get { return m_ReviewCount; }
            set { m_ReviewCount = value; }
        }

        private string m_ProductStorageDay = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 ProductStorageDay 属性值
        /// </summary>
        [DataAttribute(fieldName: "ProductStorageDay", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("保质期提醒")]
        public string ProductStorageDay
        {
            get { return m_ProductStorageDay; }
            set { m_ProductStorageDay = value; }
        }

        private int m_DeliveryParttern = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 DeliveryParttern 属性值
        /// </summary>
        [DataAttribute(fieldName: "DeliveryParttern", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("配送方式")]
        public int DeliveryParttern
        {
            get { return m_DeliveryParttern; }
            set { m_DeliveryParttern = value; }
        }

        #endregion


        #region 双计量

        private int m_AuxMeasurementUnit = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 AuxMeasurementUnit 属性值,描述:辅计量单位
        /// </summary>
        [DataAttribute(fieldName: "AuxMeasurementUnit", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("辅计量单位")]
        public int AuxMeasurementUnit
        {
            get { return m_AuxMeasurementUnit; }
            set { m_AuxMeasurementUnit = value; }
        }

        private int m_MainAuxRatio = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 MainAuxRatio 属性值,描述:主辅比例
        /// </summary>
        [DataAttribute(fieldName: "MainAuxRatio", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("主辅比例")]
        public int MainAuxRatio
        {
            get { return m_MainAuxRatio; }
            set { m_MainAuxRatio = value; }
        }

        private bool m_IsWeigh = false;
        /// <summary>
        /// 获取或设置 IsWeigh 属性值,描述:是否称重
        /// </summary>
        [DataAttribute(fieldName: "IsWeigh", dataType: DbType.Boolean, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: false)]
        
        [Description("是否称重")]
        public bool IsWeigh
        {
            get { return m_IsWeigh; }
            set { m_IsWeigh = value; }
        }
        #endregion


        private string m_BarCode = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 BarCode 属性值
        /// </summary>
        [DataAttribute(fieldName: "BarCode", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("条形码")]
        public string BarCode
        {
            get { return m_BarCode; }
            set { m_BarCode = value; }
        }


        private string m_InternalBarCode = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 InternalBarCode 属性值
        /// </summary>
        [DataAttribute(fieldName: "InternalBarCode", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("店内码")]
        public string InternalBarCode
        {
            get { return m_InternalBarCode; }
            set { m_InternalBarCode = value; }
        }

        private int m_ProductSysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 ProductSysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "ProductSysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("商品编号")]
        public int ProductSysNo
        {
            get { return m_ProductSysNo; }
            set { m_ProductSysNo = value; }
        }

        private int m_CloneProductSysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 CloneProductSysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "CloneProductSysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("克隆商品编号")]
        public int CloneProductSysNo
        {
            get { return m_CloneProductSysNo; }
            set { m_CloneProductSysNo = value; }
        }

        private int m_ApplicantSysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 ApplicantSysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "ApplicantSysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("申请人编号")]
        public int ApplicantSysNo
        {
            get { return m_ApplicantSysNo; }
            set { m_ApplicantSysNo = value; }
        }

        private DateTime m_ApplicantTime = AppConstCommon.DateTimeNull;
        /// <summary>
        /// 获取或设置 ApplicantTime 属性值
        /// </summary>
        [DataAttribute(fieldName: "ApplicantTime", dataType: DbType.DateTime, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: null)]
        
        [Description("申请时间")]
        public DateTime ApplicantTime
        {
            get { return m_ApplicantTime; }
            set { m_ApplicantTime = value; }
        }

        private int m_AuditUserSysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 AuditUserSysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "AuditUserSysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("审核人编号")]
        public int AuditUserSysNo
        {
            get { return m_AuditUserSysNo; }
            set { m_AuditUserSysNo = value; }
        }

        private DateTime m_AuditTime = AppConstCommon.DateTimeNull;
        /// <summary>
        /// 获取或设置 AuditTime 属性值
        /// </summary>
        [DataAttribute(fieldName: "AuditTime", dataType: DbType.DateTime, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: null)]
        
        [Description("审核时间")]
        public DateTime AuditTime
        {
            get { return m_AuditTime; }
            set { m_AuditTime = value; }
        }

        private int m_AuditStatus = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 AuditStatus 属性值
        /// </summary>
        [DataAttribute(fieldName: "AuditStatus", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("审核状态")]
        public int AuditStatus
        {
            get { return m_AuditStatus; }
            set { m_AuditStatus = value; }
        }

        private int m_ApplyType = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 ApplyType 属性值
        /// </summary>
        [DataAttribute(fieldName: "ApplyType", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("申请类型")]
        public int ApplyType
        {
            get { return m_ApplyType; }
            set { m_ApplyType = value; }
        }

        private int m_AuditPrivilege = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 AuditPrivilege 属性值
        /// </summary>
        [DataAttribute(fieldName: "AuditPrivilege", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("当前审核权限")]
        public int AuditPrivilege
        {
            get { return m_AuditPrivilege; }
            set { m_AuditPrivilege = value; }
        }

        private int m_WarmHintWeight = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 WarmHintWeight 属性值
        /// </summary>
        [DataAttribute(fieldName: "WarmHintWeight", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("温馨提示权重")]
        public int WarmHintWeight
        {
            get { return m_WarmHintWeight; }
            set { m_WarmHintWeight = value; }
        }

        private string m_WarmHint = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 WarmHint 属性值
        /// </summary>
        [DataAttribute(fieldName: "WarmHint", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("温馨提示")]
        public string WarmHint
        {
            get { return m_WarmHint; }
            set { m_WarmHint = value; }
        }

        private string m_TaxCode = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 TaxCode 属性值
        /// </summary>
        [DataAttribute(fieldName: "TaxCode", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("税务编码")]
        public string TaxCode
        {
            get { return m_TaxCode; }
            set { m_TaxCode = value; }
        }

        private int m_TaxC3SysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 TaxC3SysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "TaxC3SysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("税务类目")]
        public int TaxC3SysNo
        {
            get { return m_TaxC3SysNo; }
            set { m_TaxC3SysNo = value; }
        }

        private int m_WarningDay = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 WarningDay 属性值,描述:预警期
        /// </summary>
        [DataAttribute(fieldName: "WarningDay", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("预警期")]
        public int WarningDay
        {
            get { return m_WarningDay; }
            set { m_WarningDay = value; }
        }

        private int m_ProductAreaSysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 ProductAreaSysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "ProductAreaSysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("产地SysNo")]
        public int ProductAreaSysNo
        {
            get { return m_ProductAreaSysNo; }
            set { m_ProductAreaSysNo = value; }
        }

        private string m_ProducingArea = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 ProducingArea 属性值
        /// </summary>
        [DataAttribute(fieldName: "ProducingArea", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("产地")]
        public string ProducingArea
        {
            get { return m_ProducingArea; }
            set { m_ProducingArea = value; }
        }

        private int m_OriginAreaSysNo = AppConstCommon.IntNull;
        /// <summary>
        /// 获取或设置 OriginAreaSysNo 属性值
        /// </summary>
        [DataAttribute(fieldName: "OriginAreaSysNo", dataType: DbType.Int32, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.IntNull)]
        
        [Description("原料产地编号")]
        public int OriginAreaSysNo
        {
            get { return m_OriginAreaSysNo; }
            set { m_OriginAreaSysNo = value; }
        }

        private string m_OriginArea = AppConstCommon.StringNull;
        /// <summary>
        /// 获取或设置 OriginArea   属性值
        /// </summary>
        [DataAttribute(fieldName: "OriginArea", dataType: DbType.String, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: AppConstCommon.StringNull)]
        
        [Description("原料产地")]
        public string OriginArea
        {
            get { return m_OriginArea; }
            set { m_OriginArea = value; }
        }

        private bool m_IsDirectPurchase = false;
        /// <summary>
        /// 获取或设置 IsDirectPurchase 属性值
        /// </summary>
        [DataAttribute(fieldName: "IsDirectPurchase", dataType: DbType.Boolean, isPrimaryKey: false, isIdentity: false,
            isAllowNull: true, needInsert: true, needUpdate: true, defaultValue: false)]
        
        [Description("是否直采")]
        public bool IsDirectPurchase
        {
            get { return m_IsDirectPurchase; }
            set { m_IsDirectPurchase = value; }
        }
    }
}
