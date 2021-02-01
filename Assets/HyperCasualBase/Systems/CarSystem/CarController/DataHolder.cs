using System.Collections.Generic;

public static class DataHolder
{
    public static Dictionary<Side, ISensor> SensorInitializerPairs = new Dictionary<Side, ISensor>()
    {
        {Side.FRONT, new StraightSensorData()},
        {Side.BACK, new StraightSensorData() },
        {Side.LEFT, new StraightSensorData() },
        {Side.RIGHT, new StraightSensorData() },

        {Side.FRONTLEFTDIAGONAL, new DiagonalSensorData() },
        {Side.FRONTRIGHTDIAGONAL, new DiagonalSensorData() },
        {Side.BACKLEFTDIAGONAL, new DiagonalSensorData() },
        {Side.BACKRIGHTTDIAGONAL, new DiagonalSensorData() }
    };
}


