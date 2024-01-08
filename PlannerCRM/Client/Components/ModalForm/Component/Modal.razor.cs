using System.ComponentModel;

namespace PlannerCRM.Client.Components.ModalForm.Component;

public partial class Modal : ComponentBase
{
    [Parameter] public string Size { get; set; } = ModalSize.LARGE;
    [Parameter] public string Title { get; set; } = Titles.DEFAULT_MODAL_TITLE;
    
    [Parameter] public RenderFragment Header { get; set; }
    [Parameter] public RenderFragment Body { get; set; }
    [Parameter] public RenderFragment Footer { get; set; }
  
    private bool _isCancelClicked = false;

    private void CloseModal()
        => _isCancelClicked = !_isCancelClicked;
}