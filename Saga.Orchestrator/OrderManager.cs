using Saga.Orchestrator.Controllers;

namespace Saga.Orchestrator
{
    public class OrderManager(IOrderProxy orderProxy, IInventoryProxy inventoryProxy, INotifierProxy notifierProxy) : IOrderManager
    {
        enum OrderTransactionState
        {
            NotStarted,
            OrderCreated,
            OrderCancelled,
            OrderCreateFailed,
            InventoryUpdated,
            InventoryUpdatedFailed,
            InventoryRolledback,
            NotifierSent,
            NotifierSendFailed
        }

        enum OrderAction
        {
            CreateOrder,
            CancelOrder,
            UpdateInventory,
            RollbackInventory,
            SendNotifier, 
            RetrySendNotifier
        }

        public bool CreateOrder(Order input)
        {
            var orderStateMachine = new Stateless.StateMachine<OrderTransactionState, OrderAction>(OrderTransactionState.NotStarted);

            int orderId = -1;
            bool isOrderSuccess = false;
            orderStateMachine.Configure(OrderTransactionState.NotStarted)
                .PermitDynamicAsync(OrderAction.CreateOrder, async () => 
                { 
                     (orderId, isOrderSuccess) = await orderProxy.CreateOrderAsync(input);
                    return isOrderSuccess ? OrderTransactionState.OrderCreated : OrderTransactionState.OrderCreateFailed;
                });

            orderStateMachine.Configure(OrderTransactionState.OrderCreated)
                .PermitDynamicAsync(OrderAction.UpdateInventory, async () => 
                {
                    var (inventoryId, isSuccess) = await inventoryProxy.UpdateInventoryAsync(input);
                    return isSuccess ? OrderTransactionState.InventoryUpdated  : OrderTransactionState.InventoryUpdatedFailed;
                })
                .OnEntry(() => orderStateMachine.Fire(OrderAction.UpdateInventory));

            orderStateMachine.Configure(OrderTransactionState.InventoryUpdated)
                .PermitDynamicAsync(OrderAction.SendNotifier, async () =>
                {
                    var (notifierId, isSuccess) = await notifierProxy.SendAsync(input);
                    return isSuccess ? OrderTransactionState.NotifierSent : OrderTransactionState.NotifierSendFailed;
                })
                .OnEntry(() => orderStateMachine.Fire(OrderAction.SendNotifier));

            orderStateMachine.Configure(OrderTransactionState.InventoryUpdatedFailed)
                .PermitDynamic(OrderAction.RollbackInventory, () =>
                {
                    inventoryProxy.DeleteInventory(orderId);
                    return OrderTransactionState.InventoryRolledback;
                })
                .OnEntry(() => orderStateMachine.Fire(OrderAction.RollbackInventory));

            orderStateMachine.Configure(OrderTransactionState.InventoryRolledback)
                .PermitDynamic(OrderAction.CancelOrder, () =>
                {
                    orderProxy.DeleteOrder(orderId);
                    return OrderTransactionState.OrderCancelled;
                })
                .OnEntry(() => orderStateMachine.Fire(OrderAction.CancelOrder));

            orderStateMachine.Fire(OrderAction.CreateOrder);
            return orderStateMachine.State == OrderTransactionState.NotifierSent;
        }

    }

}
