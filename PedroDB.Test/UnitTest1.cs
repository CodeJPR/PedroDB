using PedroEngine;
using Xunit;

namespace PedroDB.Test {
    public class UnitTest1 {
        [Fact]
        public void Test1() {
            Engine db = new("./db");
        }
    }
}