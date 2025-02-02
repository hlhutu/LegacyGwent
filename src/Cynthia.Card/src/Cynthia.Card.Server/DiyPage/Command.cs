using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Cynthia.Card.Server;

public class Command
{
    public static IMongoClient client;
    static IMongoDatabase db;
    public static IMongoCollection<DiyCardInfo> diyCardCollection;
    public static GwentDatabaseService _dbServer;
    public static void MongodbConnect(GwentDatabaseService dbServer)
    {
        _dbServer = dbServer;
        client = dbServer.GetMongoClient();
        db = client.GetDatabase("Web");
        diyCardCollection = db.GetCollection<DiyCardInfo>("DiyCards");
    }
    public static void AddDiyCardInfos(DiyCardInfo diyCard)
    {
        int uid = diyCardCollection.AsQueryable().Count();
        diyCard.uid = uid;
        diyCard.lastEditedDate = DateTime.Now;
        diyCard.commits = new List<DiyCardInfo.Commit> { };
        diyCard.likeList = new List<string> { };
        diyCard.dislikeList = new List<string> { };
        diyCardCollection.InsertOne(diyCard);
    }
    public static void GetDiyCardsInfo()
    {
        Info.diyCardInfo = diyCardCollection.AsQueryable().ToList();
    }
    public static void UpdateDiyCardComment(DiyCardInfo diyCard)
    {
        var filter = Builders<DiyCardInfo>.Filter.Eq("uid", diyCard.uid);
        var update = Builders<DiyCardInfo>.Update.Set("commits", diyCard.commits);
        diyCardCollection.UpdateOne(filter, update);
    }
    public static void UpdateDiyCardLikeList(DiyCardInfo diyCard)
    {
        var filter = Builders<DiyCardInfo>.Filter.Eq("uid", diyCard.uid);
        var update = Builders<DiyCardInfo>.Update.Set("likeList", diyCard.likeList);
        diyCardCollection.UpdateOne(filter, update);
    }
    public static void UpdateDiyCardDislikeList(DiyCardInfo diyCard)
    {
        var filter = Builders<DiyCardInfo>.Filter.Eq("uid", diyCard.uid);
        var update = Builders<DiyCardInfo>.Update.Set("dislikeList", diyCard.dislikeList);
        diyCardCollection.UpdateOne(filter, update);
    }
    public static bool Login(string username, string password)
    {
        var user = _dbServer.Login(username, password);
        return user != null;
    }
}