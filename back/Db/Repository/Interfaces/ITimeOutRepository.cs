public interface ITimeOutRepository
{
    public bool isTimeOut(string origin);
    public void setTimeOut(string origin, int timeOut);
    public List<string> listOfTimeOuts();
}
