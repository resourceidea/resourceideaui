namespace EastSeat.ResourceIdea.Domain.Subscriptions.Enums
{
    public enum SubscriptionStatus
    {
        /// <summary>
        /// Subscription is not activated.
        /// </summary>
        Inactive,

        /// <summary>
        /// Subscription has been activated and in use.
        /// </summary>
        Active,

        /// <summary>
        /// Subscription has been suspended from use.
        /// </summary>
        Suspended,

        /// <summary>
        /// Subscription has been cancelled by the tenant.
        /// </summary>
        Canceled
    }
}