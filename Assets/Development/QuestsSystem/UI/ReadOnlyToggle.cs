using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ReadOnlyToggle : Toggle
{
    protected override void ExecuteDefaultAction(EventBase evt)
    {
        //Debug.Log("ReadOnlyToggle " + evt.GetType().Name + " executed");
        if (evt is MouseDownEvent or MouseUpEvent or PointerDownEvent or PointerUpEvent or ClickEvent)
        {
            Debug.Log(" - Intercepted " + evt.GetType().Name);
            evt.StopImmediatePropagation();
            evt.PreventDefault();
            return;
        }
        else
        {
            base.ExecuteDefaultAction(evt);
            
        }
    }

    public new class UxmlFactory : UnityEngine.UIElements.UxmlFactory<ReadOnlyToggle, ReadOnlyToggle.UxmlTraits>
    {
    }

    /// <summary>
    ///        <para>
    /// Defines UxmlTraits for the Toggle.
    /// </para>
    ///      </summary>
    public new class UxmlTraits : BaseFieldTraits<bool, UxmlBoolAttributeDescription>
    {
        private UxmlStringAttributeDescription m_Text;

        /// <summary>
        ///        <para>
        /// Initializes Toggle properties using values from the attribute bag.
        /// </para>
        ///      </summary>
        /// <param name="ve">The object to initialize.</param>
        /// <param name="bag">The attribute bag.</param>
        /// <param name="cc">The creation context; unused.</param>
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((BaseBoolField) ve).text = this.m_Text.GetValueFromBag(bag, cc);
        }

        public UxmlTraits()
        {
            UxmlStringAttributeDescription attributeDescription = new UxmlStringAttributeDescription();
            attributeDescription.name = "text";
            this.m_Text = attributeDescription;
            // ISSUE: explicit constructor call
            ;
        }
    }
}
