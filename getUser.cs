//this method retrieves single entity. 
 public static Entity RetrieveSingleEntity(string entityGuid, string entityLogicaName, IOrganizationService service)
        {
            try
            {
                Entity q_entity = new Entity();
                q_entity = service.Retrieve(entityLogicaName, new Guid(entityGuid), new ColumnSet(true));
                return q_entity;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
		
//This us a sample workflow that returns curent user asynchronously. You can change this to a synchronous or asynchronous plugin
 public class GetCurrentUser : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            ExceptionHelper exceptionHelper = new ExceptionHelper();
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                Guid userId = context.InitiatingUserId;
                EntityReference user = RetrieveSingleEntity(userId.ToString(), "systemuser", service).ToEntityReference(); //
                CurrentUser.Set(executionContext, new EntityReference("systemuser", userId));
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                InvalidPluginExecutionException newException = new Microsoft.Xrm.Sdk.InvalidPluginExecutionException(
                       String.Format("A Soap Exception occurred in the {0} plug-in: ", this.GetType().ToString()) + soapEx.Detail.InnerText, soapEx);
                exceptionHelper.HandleException(newException);
                throw newException;
            }
            catch (Exception ex)
            {
                InvalidPluginExecutionException newException = new Microsoft.Xrm.Sdk.InvalidPluginExecutionException(
                      String.Format("An Unhandled Exception occurred in the {0} plug-in: ", this.GetType().ToString()) + ex.Message, ex);
                exceptionHelper.HandleException(newException);
                throw newException;
            }
        }
        [Output("currentUser")]
        [ReferenceTarget("systemuser")]
        public OutArgument<EntityReference> CurrentUser { get; set; }
    }