﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartMobileWMS.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Parameters {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Parameters() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SmartMobileWMS.Resources.Parameters", typeof(Parameters).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO ΓενικόςΜετρητής (Oid, Μετρητής, Τιμή) VALUES((Convert(uniqueidentifier, N&apos;{0}&apos;)),&apos;ΜετρητήςRF&apos;, 0).
        /// </summary>
        internal static string addCounter {
            get {
                return ResourceManager.GetString("addCounter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE From ΚίνησηΘέσης Where Διαχείρηση = &apos;{0}&apos;
        ///.
        /// </summary>
        internal static string deleteMovement {
            get {
                return ResourceManager.GetString("deleteMovement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE From ΠεριεχόμεναΠαλέτας where Παλέτα = &apos;{0}&apos;.
        /// </summary>
        internal static string deletePaletteContent {
            get {
                return ResourceManager.GetString("deletePaletteContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE From RFΓραμμέςΑγορών where RFΠωλήσεις = &apos;{0}&apos;.
        /// </summary>
        internal static string deletePurchaseLine {
            get {
                return ResourceManager.GetString("deletePurchaseLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM RFΑπογραφή WHERE Oid =&apos;{0}&apos;.
        /// </summary>
        internal static string deleteRFCensusWithID {
            get {
                return ResourceManager.GetString("deleteRFCensusWithID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE From RFΓραμμέςΠωλήσεων where RFΠωλήσεις = &apos;{0}&apos;.
        /// </summary>
        internal static string deleteRFLineWithID {
            get {
                return ResourceManager.GetString("deleteRFLineWithID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Oid, Περιγραφή FROM ΑποθηκευτικόςΧώρος where GCRecord is null FOR JSON PATH.
        /// </summary>
        internal static string getAllStorage {
            get {
                return ResourceManager.GetString("getAllStorage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Μετρητής ,Τιμή FROM ΓενικόςΜετρητής where Μετρητής = &apos;Barcode&apos; and GCRecord is null.
        /// </summary>
        internal static string getBarCodeCounter {
            get {
                return ResourceManager.GetString("getBarCodeCounter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Oid ,Κωδικός,Περιγραφή FROM ΙεραρχικόΖοομ1 where GCRecord is null.
        /// </summary>
        internal static string getBrands {
            get {
                return ResourceManager.GetString("getBrands", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Oid ,JSON_QUERY((select Πελάτης.Oid , Πελάτης.Κωδικός ,Επωνυμία ,ΔιακριτικόςΤίτλος ,ΑΦΜ ,Email from Πελάτης where Oid = Πελάτης and GCRecord is null FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)) as Customer, (SELECT ΓραμμέςΕντολήςΣυλλογής.Oid,JSON_QUERY((SELECT  Θέση.Oid, Θέση.Κωδικός, Θέση.Περιγραφή, Θέση.AAPicking FROM Θέση where Oid = Θέση and GCRecord is null FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)) as Θέση,JSON_QUERY((select IIF (BarCodeΕίδους is null,(SELECT  Είδος.Oid ,Είδος.Κωδικός ,Είδος.Περιγραφή FROM [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string getCollectionCommands {
            get {
                return ResourceManager.GetString("getCollectionCommands", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Oid , Κωδικός ,Επωνυμία ,ΑΦΜ, ΔιακριτικόςΤίτλος ,Email from Πελάτης where GCRecord is null.
        /// </summary>
        internal static string getCustomers {
            get {
                return ResourceManager.GetString("getCustomers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Oid , Κωδικός ,Επωνυμία ,ΑΦΜ, ΔιακριτικόςΤίτλος ,Email from Πελάτης where Κωδικός Like &apos;%{0}%&apos; or Επωνυμία like &apos;%{0}%&apos; or ΑΦΜ like &apos;%{0}%&apos; and GCRecord is null FOR JSON PATH.
        /// </summary>
        internal static string getCustomersWithName {
            get {
                return ResourceManager.GetString("getCustomersWithName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Oid , Κωδικός ,Επωνυμία ,ΔιακριτικόςΤίτλος ,ΑΦΜ ,Email from Πελάτης where Oid =&apos;{0}&apos; and GCRecord is null.
        /// </summary>
        internal static string getCustomerWithID {
            get {
                return ResourceManager.GetString("getCustomerWithID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT [Oid],[Κωδικός],[Περιγραφή],[AAPicking] FROM Διάδρομος where [Κωδικός]=&apos;{0}&apos; and GCRecord is null.
        /// </summary>
        internal static string getHallWay {
            get {
                return ResourceManager.GetString("getHallWay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT [BarCodeΕίδους] as barcode,k.[Είδος] as Oid,Είδος.Κωδικός,iif(BarCodeΕίδους is not null,BarCodeΕίδους.Περιγραφή,Είδος.Περιγραφή) as Περιγραφή,CAST(ΠοσότηταΕγγραφής as int) as Ποσότητα ,JSON_QUERY((SELECT  Oid, Κωδικός, Περιγραφή FROM Θέση where Oid = k.Θέση and GCRecord is null FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)) as Position FROM [ΚίνησηΘέσης] k Join [Διαχείριση] d on k.[Διαχείρηση] = d.Oid join Είδος on Είδος.Oid = k.Είδος left join BarCodeΕίδους on BarCodeΕίδους.BarCode = k.BarCodeΕίδους where d. [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string getItemsFromMovement {
            get {
                return ResourceManager.GetString("getItemsFromMovement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT top(1) [Oid], Πελάτης,[Παραστατικό],Τύπος FROM Διαχείριση where Παραστατικό=&apos;{0}&apos; and GCRecord is null FOR JSON PATH,WITHOUT_ARRAY_WRAPPER.
        /// </summary>
        internal static string getManagement {
            get {
                return ResourceManager.GetString("getManagement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select Oid ,Κωδικός  ,Περιγραφή FROM Κατασκευαστής where GCRecord is null.
        /// </summary>
        internal static string getManufacturers {
            get {
                return ResourceManager.GetString("getManufacturers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Oid  ,ΙεραρχικόΖοομ1 ,Κωδικός ,Περιγραφή  FROM ΙεραρχικόΖοομ2 where ΙεραρχικόΖοομ1 =&apos;{0}&apos; and GCRecord is null.
        /// </summary>
        internal static string getModelsByBrand {
            get {
                return ResourceManager.GetString("getModelsByBrand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT [Oid],[SSCC],[Περιγραφή],[Πλάτος],[Μήκος],[Υψος], (SELECT iif([BarCodeΕίδους] is not null,[BarCodeΕίδους].[Είδος],[Είδος].Oid) as Oid,[Είδος].Κωδικός,[BarCodeΕίδους] as barcode,iif([BarCodeΕίδους] is not null,[BarCodeΕίδους].Περιγραφή,[Είδος].Περιγραφή) as Περιγραφή,CAST(Ποσότητα as int) as Ποσότητα, BarCodeΕίδους.SN FROM ΠεριεχόμεναΠαλέτας left join [Είδος] on [Είδος].Oid = ΠεριεχόμεναΠαλέτας.Είδος left join [BarCodeΕίδους] on [BarCodeΕίδους].BarCode = ΠεριεχόμεναΠαλέτας.BarCodeΕίδους where Παλέτα = [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string getPalette {
            get {
                return ResourceManager.GetString("getPalette", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT [Είδος].Oid,[Είδος].Κωδικός,[BarCodeΕίδους],iif([BarCodeΕίδους] is not null,[BarCodeΕίδους].Περιγραφή,[Είδος].Περιγραφή) as Περιγραφή,[Ποσότητα] FROM ΠεριεχόμεναΠαλέτας left join [Είδος] on [Είδος].Oid = ΠεριεχόμεναΠαλέτας.Είδος left join [BarCodeΕίδους] on [BarCodeΕίδους].BarCode = ΠεριεχόμεναΠαλέτας.BarCodeΕίδους where Παλέτα =&apos;{0}&apos; and GCRecord is null.
        /// </summary>
        internal static string getPaletteContent {
            get {
                return ResourceManager.GetString("getPaletteContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT  Oid, Κωδικός, Περιγραφή, AAPicking FROM Θέση where (Κωδικός = &apos;{0}&apos; or Περιγραφή = &apos;{0}&apos;) and GCRecord is null FOR JSON PATH,WITHOUT_ARRAY_WRAPPER.
        /// </summary>
        internal static string getPosition {
            get {
                return ResourceManager.GetString("getPosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from (SELECT [Θέση],CAST(sum( IIF([ΤύποςΚίνησηςΘέσης]=1,[ΠοσότηταΕγγραφής]*-1,[ΠοσότηταΕγγραφής])) as int) as Ποσότητα FROM [ΚίνησηΘέσης] left join [Είδος] on [ΚίνησηΘέσης].Είδος = [Είδος].Oid where BarCodeΕίδους=&apos;{0}&apos; or [Είδος].[Κωδικός] =&apos;{0}&apos;  group by [Θέση] ) a left join (SELECT [Oid], [Κωδικός],[Περιγραφή] FROM [Θέση]) b on a.[Θέση] = b.Oid where a.Θέση is not null FOR JSON PATH.
        /// </summary>
        internal static string getPositionFromProduct {
            get {
                return ResourceManager.GetString("getPositionFromProduct", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Oid, Κωδικός, Περιγραφή, AAPicking FROM Θέση where (Κωδικός = &apos;{0}&apos; or Περιγραφή = &apos;{0}&apos;) and ΘέσηPiking = 1 and GCRecord is null.
        /// </summary>
        internal static string getPositionWithID {
            get {
                return ResourceManager.GetString("getPositionWithID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Περιγραφή from ΠίνακαςΔεδομένων15 where GCRecord is null.
        /// </summary>
        internal static string getPrinters {
            get {
                return ResourceManager.GetString("getPrinters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from (SELECT Είδος, BarCodeΕίδους,CAST(sum( IIF([ΤύποςΚίνησηςΘέσης]=1,[ΠοσότηταΕγγραφής]*-1,[ΠοσότηταΕγγραφής])) as int) as Ποσότητα FROM [ΚίνησηΘέσης] where Θέση = &apos;{0}&apos; and GCRecord is null  group by[Είδος], [BarCodeΕίδους] ) a left join (SELECT[Είδος].[Oid],[Κωδικός],[Είδος].[Περιγραφή] FROM [Είδος] where GCRecord is null) b on a.Είδος = b.Oid where a.BarCodeΕίδους=&apos;{1}&apos; or b.Κωδικός=&apos;{1}&apos;  FOR JSON PATH,WITHOUT_ARRAY_WRAPPER.
        /// </summary>
        internal static string getProductFromPosition {
            get {
                return ResourceManager.GetString("getProductFromPosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from (SELECT Είδος, BarCodeΕίδους as barcode,  CAST(sum( IIF([ΤύποςΚίνησηςΘέσης]=1,[ΠοσότηταΕγγραφής]*-1,[ΠοσότηταΕγγραφής])) as int) as Ποσότητα  FROM [ΚίνησηΘέσης] where Θέση = &apos;{0}&apos;  and GCRecord is null  group by[Είδος], [BarCodeΕίδους] ) a left join (SELECT[Είδος].[Oid],[Κωδικός],[Είδος].[Περιγραφή] FROM [Είδος] where GCRecord is null) b on a.Είδος = b.Oid  FOR JSON PATH.
        /// </summary>
        internal static string getProductsFromPosition {
            get {
                return ResourceManager.GetString("getProductsFromPosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT [Παραστατικό],[Είδος].Oid,[Είδος].Κωδικός,[Ποσότητα],[BarCodeΕίδους] ,iif([BarCodeΕίδους] is not null,[BarCodeΕίδους].Περιγραφή,[Είδος].Περιγραφή) as Περιγραφή FROM [maindemo].[dbo].[ΠαραστατικάΠωλήσεων] join[ΓραμμέςΠαραστατικώνΠωλήσεων] on [ΠαραστατικάΠωλήσεων].Oid =[ΓραμμέςΠαραστατικώνΠωλήσεων].ΠαραστατικάΠωλήσεων join [Είδος] on [Είδος].Oid = [ΓραμμέςΠαραστατικώνΠωλήσεων].Είδος left join [BarCodeΕίδους] on[BarCodeΕίδους].BarCode = [ΓραμμέςΠαραστατικώνΠωλήσεων].BarCodeΕίδους where [Παραστατικό] =&apos;{ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string getProductsFromSalesDoc {
            get {
                return ResourceManager.GetString("getProductsFromSalesDoc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from (SELECT  Είδος.Oid ,Κωδικός ,Περιγραφή ,barcode = null ,SN=null ,ΦΠΑ ,ΜονάδεςΜέτρησης.ΜονάδαΜέτρησης ,ΜονάδεςΜέτρησης.ΤύποςΔιάστασης ,Πλάτος ,Μήκος ,Υψος ,Κείμενο2 as ProductCode2,ΤιμήΧονδρικής ,Χρώματα = null ,Μεγέθη = null FROM Είδος left join ΜονάδεςΜέτρησης on ΜονάδεςΜέτρησης.Oid = Είδος.ΜονάδαΜέτρησης where ((Είδος.Κωδικός = &apos;{0}&apos; or Είδος.Κείμενο2 =&apos;{0}&apos;)  and (ΥποχρεωτικήΔιαχείρησηSN =&apos;false&apos; or ΥποχρεωτικήΔιαχείρησηSN is null)) and Είδος.GCRecord is null UNION SELECT Είδος as Oid ,BarC [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string getProductWithID {
            get {
                return ResourceManager.GetString("getProductWithID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM (select * from (SELECT  Είδος.Oid,Κωδικός,Περιγραφή,BarCode = null,ΦΠΑ ,ΜονάδεςΜέτρησης.ΜονάδαΜέτρησης,ΜονάδεςΜέτρησης.ΤύποςΔιάστασης,Πλάτος,Μήκος,Υψος,Κείμενο2 as ProductCode2,ΤιμήΧονδρικής,Χρώματα = null ,Μεγέθη = null FROM Είδος left join ΜονάδεςΜέτρησης on ΜονάδεςΜέτρησης.Oid = Είδος.ΜονάδαΜέτρησης where  Είδος.GCRecord is null UNION SELECT Είδος as Oid,BarCode as Κωδικός,Περιγραφή,BarCode,ΦΠΑ,ΜονάδεςΜέτρησης.ΜονάδαΜέτρησης ,ΜονάδεςΜέτρησης.ΤύποςΔιάστασης,Πλάτος,Μήκος,Υψος,Κείμενο2 as Prod [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string getProductWithID_Barcode {
            get {
                return ResourceManager.GetString("getProductWithID_Barcode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select * from (SELECT Είδος.Oid  ,Κωδικός ,Περιγραφή ,BarCode = null ,ΦΠΑ ,ΜονάδεςΜέτρησης.ΜονάδαΜέτρησης ,ΜονάδεςΜέτρησης.ΤύποςΔιάστασης ,Πλάτος ,Μήκος ,Υψος ,Κείμενο2 as ProductCode2 ,ΤιμήΧονδρικής ,Χρώματα = null ,Μεγέθη = null FROM Είδος left join ΜονάδεςΜέτρησης on ΜονάδεςΜέτρησης.Oid = Είδος.ΜονάδαΜέτρησης where ((Είδος.Κωδικός LIKE &apos;%{0}%&apos; OR  Είδος.Περιγραφή LIKE &apos;%{0}%&apos; or Είδος.Κείμενο2 LIKE &apos;%{0}%&apos;) and (ΥποχρεωτικήΔιαχείρησηSN =&apos;false&apos; or ΥποχρεωτικήΔιαχείρησηSN is null)) and Είδος.GCRecord is n [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string getProductWithName {
            get {
                return ResourceManager.GetString("getProductWithName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Oid , Κωδικός ,Επωνυμία ,ΑΦΜ, Email from [Προμηθευτής] where Oid = &apos;{0}&apos;.
        /// </summary>
        internal static string getProviderWithID {
            get {
                return ResourceManager.GetString("getProviderWithID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Oid , Κωδικός ,Επωνυμία ,ΑΦΜ, Email from Προμηθευτής where (Επωνυμία like &apos;%{0}%&apos; or ΑΦΜ like &apos;%{0}%&apos;) and GCRecord is null FOR JSON PATH.
        /// </summary>
        internal static string getProviderWithName {
            get {
                return ResourceManager.GetString("getProviderWithName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Oid,JSON_QUERY((SELECT Oid,Κωδικός,Επωνυμία,ΑΦΜ,Email FROM Προμηθευτής where Oid = Προμηθευτής FOR JSON PATH ,WITHOUT_ARRAY_WRAPPER)) as Provider,ΠαραστατικόΠρομηθευτή,Ολοκληρώθηκε,ΗμνίαΔημιουργίας FROM RFΑγορές where Oid=&apos;{0}&apos; and (Ολοκληρώθηκε = 0 or Ολοκληρώθηκε is null) and GCRecord is null FOR JSON PATH,WITHOUT_ARRAY_WRAPPER.
        /// </summary>
        internal static string getPurchase {
            get {
                return ResourceManager.GetString("getPurchase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Oid,RFΠωλήσεις ,JSON_QUERY((select * from (SELECT  Είδος.Oid ,Κωδικός ,Περιγραφή ,barcode = null ,ΦΠΑ ,ΜονάδεςΜέτρησης.ΜονάδαΜέτρησης ,ΜονάδεςΜέτρησης.ΤύποςΔιάστασης ,Πλάτος ,Μήκος ,Υψος ,Κείμενο2 as ProductCode2,ΤιμήΧονδρικής ,Χρώματα = null ,Μεγέθη = null FROM Είδος left join ΜονάδεςΜέτρησης on ΜονάδεςΜέτρησης.Oid = Είδος.ΜονάδαΜέτρησης where ((Είδος.Oid = Είδος )  and (ΥποχρεωτικήΔιαχείρησηSN =&apos;false&apos; or ΥποχρεωτικήΔιαχείρησηSN is null)) and Είδος.GCRecord is null UNION SELECT Είδος as Oid ,BarCod [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string getPurchaseLines {
            get {
                return ResourceManager.GetString("getPurchaseLines", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Oid,JSON_QUERY((SELECT Oid,Κωδικός,Επωνυμία,ΑΦΜ,Email FROM Προμηθευτής where Oid = Προμηθευτής FOR JSON PATH ,WITHOUT_ARRAY_WRAPPER)) as Provider,ΠαραστατικόΠρομηθευτή,Ολοκληρώθηκε,ΗμνίαΔημιουργίας FROM RFΑγορές where (Ολοκληρώθηκε = 0 or Ολοκληρώθηκε is null) and GCRecord is null FOR JSON PATH.
        /// </summary>
        internal static string getPurchases {
            get {
                return ResourceManager.GetString("getPurchases", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Oid ,Επωνυμία from Παραλαβών where GCRecord is null FOR JSON PATH.
        /// </summary>
        internal static string getRecievers {
            get {
                return ResourceManager.GetString("getRecievers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Oid ,Επωνυμία from Παραλαβών where Oid = &apos;{0}&apos; and GCRecord is null.
        /// </summary>
        internal static string getRecieverWithID {
            get {
                return ResourceManager.GetString("getRecieverWithID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select top(10) Oid ,JSON_QUERY((SELECT Oid, Περιγραφή FROM ΑποθηκευτικόςΧώρος where Oid =ΑποθηκευτικόςΧώρος and GCRecord is null FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)) as Storage ,JSON_QUERY((SELECT Oid, Κωδικός, Περιγραφή, AAPicking FROM Θέση where Oid = Θέση and GCRecord is null FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)) as Position ,JSON_QUERY((select IIF (BarCodeΕίδους is null,(SELECT  Είδος.Oid ,Κωδικός ,Περιγραφή FROM Είδος where Είδος.Oid = Είδος and Είδος.GCRecord is null FOR JSON PATH,WITHOUT_ARRAY_WRAPP [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string getRFCensus {
            get {
                return ResourceManager.GetString("getRFCensus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Τιμή FROM ΓενικόςΜετρητής where Μετρητής=&apos;ΜετρητήςRF&apos; and GCRecord is null FOR JSON PATH,WITHOUT_ARRAY_WRAPPER.
        /// </summary>
        internal static string getRFCounter {
            get {
                return ResourceManager.GetString("getRFCounter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Oid, RFΠωλήσεις ,JSON_QUERY((select * from (SELECT  Είδος.Oid ,Κωδικός ,Περιγραφή ,barcode = null ,ΦΠΑ ,ΜονάδεςΜέτρησης.ΜονάδαΜέτρησης ,ΜονάδεςΜέτρησης.ΤύποςΔιάστασης ,Πλάτος ,Μήκος ,Υψος ,Κείμενο2 as ProductCode2,ΤιμήΧονδρικής ,Χρώματα = null ,Μεγέθη = null FROM Είδος left join ΜονάδεςΜέτρησης on ΜονάδεςΜέτρησης.Oid = Είδος.ΜονάδαΜέτρησης where ((Είδος.Oid = Είδος )  and (ΥποχρεωτικήΔιαχείρησηSN =&apos;false&apos; or ΥποχρεωτικήΔιαχείρησηSN is null)) and Είδος.GCRecord is null UNION SELECT Είδος as Oid ,BarCo [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string getRFLinesWithRFSaleID {
            get {
                return ResourceManager.GetString("getRFLinesWithRFSaleID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT  Oid, Κωδικός, Περιγραφή FROM Θέση where GCRecord is null.
        /// </summary>
        internal static string getRFPositions {
            get {
                return ResourceManager.GetString("getRFPositions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Oid ,JSON_QUERY((select Oid,Κωδικός,Επωνυμία,ΑΦΜ,Email from Πελάτης where Oid=Πελάτης FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)) as Customer ,ΠαραστατικόΠελάτη ,UpdSmart ,Ολοκληρώθηκε ,ΗμνίαΔημιουργίας ,JSON_QUERY((select Oid,Επωνυμία from Παραλαβών where Oid=Παραλαβών FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)) as Reciever from RFΠωλήσεις where Oid=&apos;{0}&apos; and UpdSmart = &apos;false&apos; and GCRecord is null FOR JSON PATH,WITHOUT_ARRAY_WRAPPER.
        /// </summary>
        internal static string getRFSale {
            get {
                return ResourceManager.GetString("getRFSale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Oid ,JSON_QUERY((select Oid,Κωδικός,Επωνυμία,ΑΦΜ,Email from Πελάτης where Oid=Πελάτης FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)) as Customer ,ΠαραστατικόΠελάτη ,UpdSmart ,Ολοκληρώθηκε ,ΗμνίαΔημιουργίας ,JSON_QUERY((select Oid,Επωνυμία from Παραλαβών where Oid=Παραλαβών FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)) as Reciever from RFΠωλήσεις where UpdSmart = &apos;false&apos; and GCRecord is null FOR JSON PATH.
        /// </summary>
        internal static string getRFSales {
            get {
                return ResourceManager.GetString("getRFSales", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT  [Oid] ,JSON_QUERY((select Oid , Κωδικός ,Επωνυμία ,ΔιακριτικόςΤίτλος ,ΑΦΜ ,Email from Πελάτης where Oid = Πελάτης and GCRecord is null FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)) as Customer,(SELECT Είδος.Oid,Είδος.Κωδικός,CAST(Ποσότητα as int) as Ποσότητα,BarCodeΕίδους as barcode ,iif(BarCodeΕίδους is not null,BarCodeΕίδους.Περιγραφή,Είδος.Περιγραφή) as Περιγραφή FROM ΠαραστατικάΠωλήσεων join ΓραμμέςΠαραστατικώνΠωλήσεων on ΠαραστατικάΠωλήσεων.Oid =ΓραμμέςΠαραστατικώνΠωλήσεων.ΠαραστατικάΠωλήσεων join Είδο [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string getSalesDoc {
            get {
                return ResourceManager.GetString("getSalesDoc", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Oid, Περιγραφή FROM ΑποθηκευτικόςΧώρος where Oid =&apos;{0}&apos; and GCRecord is null.
        /// </summary>
        internal static string getStorageWithID {
            get {
                return ResourceManager.GetString("getStorageWithID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT a.Oid  ,a.Parent ,a.Name as NameChild  ,ΔενδρικήΟμαδοποιησηΕιδών.ID FROM HCategory a   inner join ΔενδρικήΟμαδοποιησηΕιδών on ΔενδρικήΟμαδοποιησηΕιδών.Oid = a.Oid.
        /// </summary>
        internal static string getTreeGrouping {
            get {
                return ResourceManager.GetString("getTreeGrouping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Oid, UserName,StoredPassword as Password from [User] where UserName =&apos;{0}&apos;  FOR JSON PATH ,WITHOUT_ARRAY_WRAPPER.
        /// </summary>
        internal static string getUser {
            get {
                return ResourceManager.GetString("getUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select Oid, UserName from [User] where UserName =&apos;{0}&apos;.
        /// </summary>
        internal static string getUserWithID {
            get {
                return ResourceManager.GetString("getUserWithID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO ΓραμμέςΕντολήςΣυλλογής (Oid ,Θέση ,BarCodeΕίδους ,Είδος , ΠοσότηταΕντολής,ΠοσότηταΣυλλογής, ΕντολήΣυλλογής, ΑφοράΓραμμήΕντολήςΣυλλογής) VALUES(Convert(uniqueidentifier, N&apos;{0}&apos;), Convert(uniqueidentifier, N&apos;{1}&apos;),&apos;{2}&apos;,Convert(uniqueidentifier, N&apos;{3}&apos;),&apos;{4}&apos;,&apos;{5}&apos;,Convert(uniqueidentifier, N&apos;{6}&apos;),Convert(uniqueidentifier, N&apos;{7}&apos;)).
        /// </summary>
        internal static string postCollectedToCommand {
            get {
                return ResourceManager.GetString("postCollectedToCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Διαχείριση (Oid, Τύπος, Πελάτης, Ημνία, Παραστατικό) VALUES((Convert(uniqueidentifier, N&apos;{0}&apos;)),&apos;{1}&apos;,(Convert(uniqueidentifier, N&apos;{2}&apos;)), GETDATE(),&apos;{3}&apos;); .
        /// </summary>
        internal static string postManagement {
            get {
                return ResourceManager.GetString("postManagement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Παλέτα (Oid,SSCC,Περιγραφή,Μήκος,Πλάτος,Υψος) values (&apos;{0}&apos;,&apos;{1}&apos;,&apos;{2}&apos;,&apos;{3}&apos;,&apos;{4}&apos;,&apos;{5}&apos;).
        /// </summary>
        internal static string postPalette {
            get {
                return ResourceManager.GetString("postPalette", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO ΠεριεχόμεναΠαλέτας (Oid,Παλέτα,[Είδος],[BarCodeΕίδους],[Ποσότητα]) values (&apos;{0}&apos;,&apos;{1}&apos;,&apos;{2}&apos;,{3},&apos;{4}&apos;).
        /// </summary>
        internal static string postPaletteItem {
            get {
                return ResourceManager.GetString("postPaletteItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO ΚίνησηΘέσης (Oid ,Θέση ,BarCodeΕίδους ,Είδος , ΠοσότηταΕγγραφής, ΤύποςΚίνησηςΘέσης, Διαχείρηση, Ημνία, Παλέτα) VALUES((Convert(uniqueidentifier, N&apos;{0}&apos;)), (Convert(uniqueidentifier, N&apos;{1}&apos;)),{2}, (Convert(uniqueidentifier, N&apos;{3}&apos;)),&apos;{4}&apos;,&apos;{5}&apos;,{6},GETDATE(),{7});.
        /// </summary>
        internal static string postPosition {
            get {
                return ResourceManager.GetString("postPosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO RFΑγορές (Oid, Προμηθευτής, ΠαραστατικόΠρομηθευτή, ΗμνίαΔημιουργίας) VALUES((Convert(uniqueidentifier, N&apos;{0}&apos;)), (Convert(uniqueidentifier, N&apos;{1}&apos;)), &apos;{2}&apos;, GETDATE());.
        /// </summary>
        internal static string postPurchase {
            get {
                return ResourceManager.GetString("postPurchase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO RFΓραμμέςΑγορών (Oid, RFΠωλήσεις, Είδος, Ποσότητα, BarCodeΕίδους, ΠοσότηταΔιάστασης, Μήκος, Πλάτος, Υψος) VALUES((Convert(uniqueidentifier, N&apos;{0}&apos;)),(Convert(uniqueidentifier, N&apos;{1}&apos;)),(Convert(uniqueidentifier, N&apos;{2}&apos;)), Convert(float,&apos;{3}&apos;), {4}, Convert(float,&apos;{5}&apos;), Convert(float,&apos;{6}&apos;), Convert(float,&apos;{7}&apos;), Convert(float,&apos;{8}&apos;)); .
        /// </summary>
        internal static string postPurchaseLine {
            get {
                return ResourceManager.GetString("postPurchaseLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO [Παραλαβών] (Oid, Επωνυμία) VALUES((Convert(uniqueidentifier, N&apos;{0}&apos;)),&apos;{1}&apos;).
        /// </summary>
        internal static string postReciever {
            get {
                return ResourceManager.GetString("postReciever", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO RFΑπογραφή (Oid, ΑποθηκευτικόςΧώρος, Είδος, ΧρήστηςΔημιουργίας, Ποσότητα, ΗμνίαΔημιουργίας,  Θέση, BarCodeΕίδους ,UpdSmart ,UpdWMS ,Ολοκληρώθηκε) VALUES((Convert(uniqueidentifier, N&apos;{0}&apos;)),(Convert(uniqueidentifier, N&apos;{1}&apos;)),(Convert(uniqueidentifier, N&apos;{2}&apos;)),(Convert(uniqueidentifier, N&apos;{3}&apos;)),&apos;{4}&apos;, GETDATE(),(Convert(uniqueidentifier, N&apos;{5}&apos;)),{6},0,0,1).
        /// </summary>
        internal static string postRFCensus {
            get {
                return ResourceManager.GetString("postRFCensus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO RFΓραμμέςΠωλήσεων (Oid, RFΠωλήσεις, Είδος, Ποσότητα, Θέση, OptimisticLockField, GCRecord, BarCodeΕίδους, ΠοσότηταΔιάστασης, Μήκος, Πλάτος, Υψος) VALUES((Convert(uniqueidentifier, N&apos;{0}&apos;)), (Convert(uniqueidentifier, N&apos;{1}&apos;)), (Convert(uniqueidentifier, N&apos;{2}&apos;)), Convert(float,&apos;{3}&apos;), null, &apos;1&apos;, null, {4}, Convert(float,&apos;{5}&apos;), Convert(float,&apos;{6}&apos;), Convert(float,&apos;{7}&apos;), Convert(float,&apos;{8}&apos;)); .
        /// </summary>
        internal static string postRFLine {
            get {
                return ResourceManager.GetString("postRFLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO RFΠωλήσεις (Oid, ΑποθηκευτικόςΧώρος, Πελάτης, ΠαραστατικάΠωλήσεων, ΠαραστατικόΠελάτη,  Διαχείριση, UpdSmart, Ολοκληρώθηκε, ΗμνίαΔημιουργίας, ΑυτόματηΔιαγραφήΠαραστατικών, OptimisticLockField, GCRecord, Παραλαβών)  VALUES((Convert(uniqueidentifier, N&apos;{0}&apos;)), null, (Convert(uniqueidentifier, N&apos;{1}&apos;)) , null, &apos;{2}&apos;, null, &apos;0&apos;, &apos;0&apos;, GETDATE(), &apos;0&apos;, &apos;1&apos;, null,  {3}); .
        /// </summary>
        internal static string postRFSale {
            get {
                return ResourceManager.GetString("postRFSale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE ΓενικόςΜετρητής SET Τιμή = {0} WHERE Μετρητής = &apos;Barcode&apos;.
        /// </summary>
        internal static string putBarCodeCounter {
            get {
                return ResourceManager.GetString("putBarCodeCounter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to update ΓραμμέςΕντολήςΣυλλογής set ΠοσότηταΣυλλογής = {0} where Oid =&apos;{1}&apos;.
        /// </summary>
        internal static string putCollectedToCommand {
            get {
                return ResourceManager.GetString("putCollectedToCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Διάδρομος SET AAPicking = &apos;{1}&apos; where [Κωδικός]=&apos;{0}&apos;.
        /// </summary>
        internal static string putHallWay {
            get {
                return ResourceManager.GetString("putHallWay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE [Θέση] SET AAPicking = &apos;{1}&apos;,Διάδρομος = &apos;{2}&apos; where [Κωδικός]=&apos;{0}&apos;.
        /// </summary>
        internal static string putPosition {
            get {
                return ResourceManager.GetString("putPosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE RFΑγορές SET Προμηθευτής = &apos;{0}&apos;,ΠαραστατικόΠρομηθευτή = &apos;{1}&apos;, Ολοκληρώθηκε = &apos;{2}&apos;, UpdSmart = &apos;{2}&apos; WHERE Oid = &apos;{3}&apos; .
        /// </summary>
        internal static string putPurchase {
            get {
                return ResourceManager.GetString("putPurchase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE RFΑπογραφή SET Ποσότητα = &apos;{0}&apos; WHERE Oid = &apos;{1}&apos; .
        /// </summary>
        internal static string putRFCensus {
            get {
                return ResourceManager.GetString("putRFCensus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE ΓενικόςΜετρητής  SET Τιμή = &apos;{0}&apos; WHERE Μετρητής = &apos;ΜετρητήςRF&apos;.
        /// </summary>
        internal static string putRFCounter {
            get {
                return ResourceManager.GetString("putRFCounter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE RFΠωλήσεις SET Πελάτης = &apos;{0}&apos; , Ολοκληρώθηκε = &apos;{1}&apos; ,  UpdSmart = &apos;{2}&apos; , Παραλαβών = {3} WHERE Oid = &apos;{4}&apos; .
        /// </summary>
        internal static string putRFSale {
            get {
                return ResourceManager.GetString("putRFSale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Διάδρομος SET AAPicking = 0.
        /// </summary>
        internal static string setHallAAToZero {
            get {
                return ResourceManager.GetString("setHallAAToZero", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Θέση SET AAPicking = 0, Διάδρομος = null.
        /// </summary>
        internal static string setPositionAAToZero {
            get {
                return ResourceManager.GetString("setPositionAAToZero", resourceCulture);
            }
        }
    }
}
