namespace OwnGraphicsAgain
{
	public interface IConstructorOwner<TArg>
	{
		void CallConstructor(TArg arg);
	}
}
