using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using PlannerCRM.Client.Services.Crud;
using PlannerCRM.Shared.Models;
using PlannerCRM.Shared.DTOs.Workorder.Views;
using static PlannerCRM.Shared.Constants.ConstantValues;

namespace PlannerCRM.Client.Pages.OperationManager.Home;

[Authorize(Roles = nameof(Roles.OPERATION_MANAGER))]
public partial class OperationManager
{
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private OperationManagerCrudService OperationManagerService { get; set; }

    private List<WorkOrderViewDto> _WorkOrders = new();
    private WorkOrderViewDto _CurrentWorkOrder { get; set; } = new();
    private bool _TrIsClicked = false;
    private int RowCounter { get; set; } = ONE;

    public bool _IsCreateWorkOrderClicked { get; set; }
    public bool _IsEditWorkOrderClicked { get; set; }
    public bool _IsDeleteWorkOrderClicked { get; set; }
    
    public bool _IsCreateActivityClicked { get; set; }

    public int _WorkOrderId { get; set; } 
    public int _ActivityId { get; set; } 

    private int _CollectionSize { get; set; }
    private int _TotalPageNumbers { get; set; }
    private int _PageNumber { get; set; } = ONE;
    private int _Limit { get; set; } = ZERO;
    private int _Offset { get => PAGINATION_LIMIT; }

    protected override async Task OnInitializedAsync() {
        _WorkOrders = await OperationManagerService.GetCollectionPaginated();
        _CollectionSize = await OperationManagerService.GetCollectionSize();
        _TotalPageNumbers = (_CollectionSize % PAGINATION_LIMIT) == ZERO
            ? (_CollectionSize / PAGINATION_LIMIT)
            : (_CollectionSize / PAGINATION_LIMIT) + ONE;
    }
    
    public async Task Previous(int pageNumber) {
        if (_Limit <= PAGINATION_LIMIT) {
            _Limit = ZERO;
            _PageNumber = ONE;
        } else {
            _Limit -= (_Limit - PAGINATION_LIMIT);
            _PageNumber--;
        }
        _WorkOrders = await OperationManagerService.GetCollectionPaginated(_Limit, _Offset);
    }

    public async Task Next(int pageNumber) {
        if (_Limit < (_TotalPageNumbers + PAGINATION_LIMIT)) {
            _Limit += PAGINATION_LIMIT;
            _PageNumber++; 
        } else {
            _Limit = _TotalPageNumbers;
            _PageNumber = _TotalPageNumbers;
        }
        _WorkOrders = await OperationManagerService.GetCollectionPaginated(_Limit, _Offset);
    }
    private void OnClickTableRow(int workorderId) {
        _TrIsClicked = !_TrIsClicked;
        _CurrentWorkOrder = _WorkOrders.Find(wo => wo.Id == workorderId);
    }

    private void OnClickAddWorkOrder() {
       _IsCreateWorkOrderClicked = !_IsCreateWorkOrderClicked;
    }

    private void OnClickAddActivity() {
       _IsCreateActivityClicked = !_IsCreateActivityClicked;
    }

    private void OnClickEdit(int id) {
        _IsEditWorkOrderClicked = !_IsEditWorkOrderClicked;
        _WorkOrderId = id;
    }

    public void OnClickDelete(int id) {
        _IsDeleteWorkOrderClicked = !_IsDeleteWorkOrderClicked;
        _WorkOrderId = id;
    }
}