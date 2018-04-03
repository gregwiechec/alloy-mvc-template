To group properties use GroupingHeader attribute. Attribute should be placed on the property.

[GroupingHeader("Sample group")]
[Display(GroupName = "Company", Order = 1)]
public virtual string TestProperty1 { get; set; }


Attrbiute can be used with second parameter which is class name:

[GroupingHeader("Sample group", "custom-class")]
[Display(GroupName = "Company", Order = 1)]
public virtual string TestProperty1 { get; set; }

In Alloy demo custom edit mode styles are in "\ClientResources\Styles\Styles.css"


The third parameter of GroupingHeader is tag.

[GroupingHeader("Sample group", "custom-class", "span")]
[Display(GroupName = "Company", Order = 1)]
public virtual string TestProperty1 { get; set; }

By default header is rendered using "h2".
