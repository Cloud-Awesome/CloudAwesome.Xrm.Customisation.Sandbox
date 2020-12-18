## Build Number: {{buildDetails.id}}

{{forEach commits}} {{if isFirst}} Associated commits {{/if}}

**ID {{this.id}}**
Message: {{this.message}}
Commited by: {{this.author.displayName}} 
{{/forEach}}

{{forEach workItems}} {{if isFirst}} **Associated Work Items** {{/if}}

**{{this.id}} {{lookup this.fields 'System.Title'}}**

Type {{lookup this.fields 'System.WorkItemType'}}
Tags {{lookup this.fields 'System.Tags'}}
Assigned {{#with (lookup this.fields 'System.AssignedTo')}} {{displayName}} {{/with}} 

{{/forEach}}