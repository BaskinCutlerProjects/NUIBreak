﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.0.30319.17929.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class Stretches {
    
    private StretchesStretch[] stretchField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Stretch")]
    public StretchesStretch[] Stretch {
        get {
            return this.stretchField;
        }
        set {
            this.stretchField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class StretchesStretch {
    
    private string descriptionField;
    
    private StretchesStretchRule[] ruleField;
    
    private string nameField;
    
    /// <remarks/>
    public string Description {
        get {
            return this.descriptionField;
        }
        set {
            this.descriptionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Rule")]
    public StretchesStretchRule[] Rule {
        get {
            return this.ruleField;
        }
        set {
            this.ruleField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class StretchesStretchRule {
    
    private StretchesStretchRuleType typeField;
    
    private string joint1Field;
    
    private string joint2Field;
    
    private StretchesStretchRuleOperator operatorField;
    
    private StretchesStretchRuleAxis axisField;
    
    private float rangeField;
    
    private bool rangeFieldSpecified;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public StretchesStretchRuleType Type {
        get {
            return this.typeField;
        }
        set {
            this.typeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Joint1 {
        get {
            return this.joint1Field;
        }
        set {
            this.joint1Field = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Joint2 {
        get {
            return this.joint2Field;
        }
        set {
            this.joint2Field = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public StretchesStretchRuleOperator Operator {
        get {
            return this.operatorField;
        }
        set {
            this.operatorField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public StretchesStretchRuleAxis Axis {
        get {
            return this.axisField;
        }
        set {
            this.axisField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public float Range {
        get {
            return this.rangeField;
        }
        set {
            this.rangeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool RangeSpecified {
        get {
            return this.rangeFieldSpecified;
        }
        set {
            this.rangeFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public enum StretchesStretchRuleType {
    
    /// <remarks/>
    Compare,
    
    /// <remarks/>
    Distance,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public enum StretchesStretchRuleOperator {
    
    /// <remarks/>
    LT,
    
    /// <remarks/>
    GT,
    
    /// <remarks/>
    EQ,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public enum StretchesStretchRuleAxis {
    
    /// <remarks/>
    X,
    
    /// <remarks/>
    Y,
    
    /// <remarks/>
    Z,
    
    /// <remarks/>
    All,
}
