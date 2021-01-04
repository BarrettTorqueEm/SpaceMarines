using SimpleTcp;

class Client {
    public string IPport { get; private set; }
    public string UName { get; private set; }

    public Client(string IPport) {
        this.IPport = IPport;
    }

    public void SetName(string Uname) {
        if (UName == null)
            UName = Uname;
    }
}