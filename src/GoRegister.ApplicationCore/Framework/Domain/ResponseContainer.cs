namespace GoRegister.ApplicationCore.Framework.Domain
{
    public class ResponseContainer<TModel, TViewModel>
    {
        public ResponseContainer(TModel model, TViewModel viewModel)
        {
            Model = model;
            ViewModel = viewModel;
        }


        public TModel Model { get; set; }
        public TViewModel ViewModel {get;set;}
    }

    public static class ResponseContainer
    {
        public static ResponseContainer<TModel, TViewModel> Create<TModel, TViewModel>(TModel model, TViewModel viewModel) 
            => new ResponseContainer<TModel, TViewModel>(model, viewModel);

        public static Result<ResponseContainer<TModel, TViewModel>> Ok<TModel, TViewModel>(TModel model, TViewModel viewModel)
            => Result.Ok(Create(model, viewModel));
    }
}
