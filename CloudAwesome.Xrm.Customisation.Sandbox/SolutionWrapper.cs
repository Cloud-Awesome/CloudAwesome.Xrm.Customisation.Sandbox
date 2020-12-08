using System;

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.Sandbox
{
    public static class SolutionWrapper
    {
        public static void AddSolutionComponent(IOrganizationService client, string solutionName, 
            Guid solutionComponentId, ComponentType solutionComponentTypeCode)
        {
            client.Execute(new AddSolutionComponentRequest
            {
                ComponentType = (int)solutionComponentTypeCode,
                ComponentId = solutionComponentId,
                SolutionUniqueName = solutionName
            });
        }


    }

    // TODO - This Enum is now woefully out of date ;)
    public enum ComponentType
    {
        All = 0,
        Entity = 1,
        Attribute = 2,
        Relationship = 3,
        AttributePicklistValue = 4,
        AttributeLookupValue = 5,
        ViewAttribute = 6,
        LocalizedLabel = 7,
        RelationshipExtraCondition = 8,
        OptionSet = 9,
        EntityRelationship = 10,
        EntityRelationshipRole = 11,
        EntityRelationshipRelationships = 12,
        ManagedProperty = 13,
        Role = 20,
        RolePrivilege = 21,
        DisplayString = 22,
        DisplayStringMap = 23,
        Form = 24,
        Organization = 25,
        SavedQuery = 26,
        Workflow = 29,
        Report = 31,
        ReportEntity = 32,
        ReportCategory = 33,
        ReportVisibility = 34,
        Attachment = 35,
        EmailTemplate = 36,
        ContractTemplate = 37,
        KBArticleTemplate = 38,
        MailMergeTemplate = 39,
        DuplicateRule = 44,
        DuplicateRuleCondition = 45,
        EntityMap = 46,
        AttributeMap = 47,
        RibbonCommand = 48,
        RibbonContextGroup = 49,
        RibbonCustomization = 50,
        RibbonRule = 52,
        RibbonTabToCommandMap = 53,
        RibbonDiff = 55,
        SavedQueryVisualization = 59,
        SystemForm = 60,
        WebResource = 61,
        SiteMap = 62,
        ConnectionRole = 63,
        HierarchyRule = 65,
        FieldSecurityProfile = 70,
        FieldPermission = 71,
        AppModule = 80,
        PluginType = 90,
        PluginAssembly = 91,
        SDKMessageProcessingStep = 92,
        SDKMessageProcessingStepImage = 93,
        ServiceEndpoint = 95,
        RoutingRule = 150,
        RoutingRuleItem = 151,
        SLA = 152,
        SLAItem = 153,
        ConvertRule = 154,
        ConvertRuleItem = 155
    }
}
