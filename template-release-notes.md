## Build: {{buildDetails.name}}

{{#forEach workItems}} {{#if isFirst}}## Associated work items {{/if}}

**{{this.id}} {{lookup this.fields 'System.Title'}}**<br/>
WIT {{lookup this.fields 'System.WorkItemType'}}<br/>
Tags {{lookup this.fields 'System.Tags'}}<br/>
{{/forEach}}

{{#forEach commits}} {{#if isFirst}}## Associated commits {{/if}}

**{{this.message}}**<br/>
ID {{this.id}}<br/>
{{/forEach}}

