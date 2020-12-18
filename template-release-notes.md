## Build Number: {{buildDetails.id}}

{{#forEach commits}} {{#if isFirst}}### Associated commits (only shown if CS) {{/if}}

** ID{{this.id}}**
Message: {{this.message}}
Commited by: {{this.author.displayName}} {{/forEach}}

{{#forEach workItems}} {{#if isFirst}}## Associated Work Items (only shown if WI) {{/if}}

{{this.id}} {{lookup this.fields 'System.Title'}}
WIT {{lookup this.fields 'System.WorkItemType'}}
Tags {{lookup this.fields 'System.Tags'}}
Assigned {{#with (lookup this.fields 'System.AssignedTo')}} {{displayName}} {{/with}} {{/forEach}}