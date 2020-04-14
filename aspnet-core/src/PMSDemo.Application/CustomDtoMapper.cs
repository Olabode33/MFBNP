using PMSDemo.PriorityAreas.Dtos;
using PMSDemo.PriorityAreas;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityParameters;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using PMSDemo.Auditing.Dto;
using PMSDemo.Authorization.Accounts.Dto;
using PMSDemo.Authorization.Delegation;
using PMSDemo.Authorization.Permissions.Dto;
using PMSDemo.Authorization.Roles;
using PMSDemo.Authorization.Roles.Dto;
using PMSDemo.Authorization.Users;
using PMSDemo.Authorization.Users.Delegation.Dto;
using PMSDemo.Authorization.Users.Dto;
using PMSDemo.Authorization.Users.Importing.Dto;
using PMSDemo.Authorization.Users.Profile.Dto;
using PMSDemo.Chat;
using PMSDemo.Chat.Dto;
using PMSDemo.DynamicEntityParameters.Dto;
using PMSDemo.Editions;
using PMSDemo.Editions.Dto;
using PMSDemo.Friendships;
using PMSDemo.Friendships.Cache;
using PMSDemo.Friendships.Dto;
using PMSDemo.Localization.Dto;
using PMSDemo.MultiTenancy;
using PMSDemo.MultiTenancy.Dto;
using PMSDemo.MultiTenancy.HostDashboard.Dto;
using PMSDemo.MultiTenancy.Payments;
using PMSDemo.MultiTenancy.Payments.Dto;
using PMSDemo.Notifications.Dto;
using PMSDemo.Organizations.Dto;
using PMSDemo.Sessions.Dto;
using PMSDemo.WebHooks.Dto;
using PMSDemo.Agencies.Dtos;
using PMSDemo.Agencies;
using PMSDemo.Deliverables.Dtos;
using PMSDemo.Deliverables;
using PMSDemo.PerformanceIndicators.Dtos;
using PMSDemo.PerformanceIndicators;
using PMSDemo.PerformanceActivities.Dtos;
using PMSDemo.PerformanceActivities;

namespace PMSDemo
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<IndicatorAttachmentDto, IndicatorAttachment>().ReverseMap();
            configuration.CreateMap<ActivityAttachmentDto, ActivityAttachment>().ReverseMap();
            configuration.CreateMap<PerformanceReviewDto, PerformanceReview>().ReverseMap();
            configuration.CreateMap<CreateOrEditPerformanceReviewDto, PerformanceReview>().ReverseMap();
            configuration.CreateMap<IndicatorYearlyTargetDto, IndicatorYearlyTarget>().ReverseMap()
                .ForMember(dto => dto.LastUpdated, opt => opt.Ignore());
            configuration.CreateMap<CreateOrEditPerformanceActivityDto, PerformanceActivity>().ReverseMap();
            configuration.CreateMap<PerformanceActivityDto, PerformanceActivity>().ReverseMap();
            configuration.CreateMap<CreateOrEditPerformanceIndicatorDto, PerformanceIndicator>().ReverseMap();
            configuration.CreateMap<PerformanceIndicatorDto, PerformanceIndicator>().ReverseMap();
            configuration.CreateMap<CreateOrEditDeliverableDto, Deliverable>()
                .ForMember(e => e.ParentId, opt => opt.MapFrom(dto => dto.MdaId))
                .ReverseMap();
            configuration.CreateMap<DeliverableDto, Deliverable>().ReverseMap()
                .ForMember(dto => dto.MdaId, opt => opt.MapFrom(e => e.ParentId));
            configuration.CreateMap<CreateOrEditMdaDto, Mda>().ReverseMap();
            configuration.CreateMap<MdaDto, Mda>().ReverseMap();
            configuration.CreateMap<CreateOrEditPriorityAreaDto, PriorityArea>().ReverseMap();
            configuration.CreateMap<PriorityAreaDto, PriorityArea>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicParameter, DynamicParameterDto>().ReverseMap();
            configuration.CreateMap<DynamicParameterValue, DynamicParameterValueDto>().ReverseMap();
            configuration.CreateMap<EntityDynamicParameter, EntityDynamicParameterDto>()
                .ForMember(dto => dto.DynamicParameterName,
                    options => options.MapFrom(entity => entity.DynamicParameter.ParameterName));
            configuration.CreateMap<EntityDynamicParameterDto, EntityDynamicParameter>();

            configuration.CreateMap<EntityDynamicParameterValue, EntityDynamicParameterValueDto>().ReverseMap();
            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();


            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}
