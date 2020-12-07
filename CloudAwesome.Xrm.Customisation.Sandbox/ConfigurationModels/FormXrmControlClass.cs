using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Sandbox.ConfigurationModels
{
    public class FormXrmControlClass
    {
        public static Guid GetFormControlClassId(Attribute attribute)
        {
            switch (attribute.DataType)
            {
                case "Integer":
                    return new Guid("C6D124CA-7EDA-4A60-AEA9-7FB8D318B68F");

                case "Boolean":
                    return new Guid("B0C6723A-8503-4FD7-BB28-C8A06AC933C2");

                case "Lookup":

                    return new Guid("270BD3DB-D9AF-4782-9025-509E298DEC0A");

                case "DateTime":
                    return new Guid("5B773807-9FB2-42DB-97C3-7A91EFF8ADFF");

                case "Decimal":
                case "Double":
                    return new Guid("C3EFE0C3-0EC6-42BE-8349-CBD9079DFD8E");

                case "Memo":
                    return new Guid("E0DECE4B-6FC8-4A8F-A065-082708572369");

                case "Money":
                    return new Guid("533B9E00-756B-4312-95A0-DC888637AC78");

                case "PartyList":
                    return new Guid("CBFB742C-14E7-4A17-96BB-1A13F7F64AA2");

                case "Picklist":
                    return new Guid("3EF39988-22BB-4f0b-BBBE-64B5A3748AEE");

                case "String":
                    switch (attribute.StringFormat)
                    {
                        case StringFormat.Email:
                            return new Guid("ADA2203E-B4CD-49BE-9DDF-234642B43B52");

                        case StringFormat.Phone:
                            return new Guid("8C10015A-B339-4982-9474-A95FE05631A5");

                        case StringFormat.Text:
                            return new Guid("4273EDBD-AC1D-40D3-9FB2-095C621B552D");

                        case StringFormat.TextArea:
                            return new Guid("E0DECE4B-6FC8-4A8F-A065-082708572369");

                        case StringFormat.TickerSymbol:
                            return new Guid("1E1FC551-F7A8-43AF-AC34-A8DC35C7B6D4");

                        case StringFormat.Url:
                            return new Guid("71716B6C-711E-476C-8AB8-5D11542BFB47");

                        default:
                            throw new NotSupportedException("String Format not supported");
                    }

                //case "Virtual":
                //    switch (attribute.GlobalOptionSet)
                //    {
                //        case "MultiSelectPicklistType":
                //            return new Guid("4AA28AB7-9C13-4F57-A73D-AD894D048B5F");

                //        default:
                //            return Guid.Empty;
                //    }

                default:
                    return Guid.Empty;
            }

        }
    }
}
