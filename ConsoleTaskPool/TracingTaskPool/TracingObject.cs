
namespace ConsoleTaskPool
{
    public class TracingObject
    {
        private int _id;

        private string _name;

        private string _content;

        private string _description;

        private int _type;

        public int GetId()
        {
            return _id;
        }

        public void SetId(int id)
        {
            this._id = id;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string name)
        {
            this._name = name;
        }

        public string GetContent()
        {
            return _content;
        }

        public void SetContent(string content)
        {
            this._content = content;
        }

        public string GetDescription()
        {
            return _description;
        }

        public void SetDescription(string description)
        {
            this._description = description;
        }

        public int GetType()
        {
            return _type;
        }

        public void SetType(int type)
        {
            this._type = type;
        }

        public override string ToString()
        {
            // TODO Auto-generated method stub
            return "id:" + _id
                         + "\t name:" + _name
                         + "\t content:" + _content
                         + "\t description:" + _description
                         + "\t type:" + _type;
        }

        public TracingObject(int id, string name, string content,
            string description, int type)
        {
//            super();
            this._id = id;
            this._name = name;
            this._content = content;
            this._description = description;
            this._type = type;
        }
    }
}
