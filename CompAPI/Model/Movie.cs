using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompAPI.Model
{
    public class Movie
    {
        public string Img { get; set; }

        public static Movie From(Content data)
        {
            return new Movie()
            {
                Img = data.image_url
            };
        }
    }

    public class MovieData
    {
        public Content[] content { get; set; }
        public string status { get; set; }
    }

    public class Content
    {
        public string[] actor { get; set; }
        public string[] age { get; set; }
        public string brief { get; set; }
        public object[] category { get; set; }
        public int comment_count { get; set; }
        public object[] content { get; set; }
        public int content_type { get; set; }
        public string[] creator { get; set; }
        public string description { get; set; }
        public string display_name { get; set; }
        public object[] dubber { get; set; }
        public object[] duration { get; set; }
        public long entity_id { get; set; }
        public string entity_type { get; set; }
        public int format_duration { get; set; }
        public object[] guest { get; set; }
        public object[] host { get; set; }
        public string image_url { get; set; }
        public object[] industry { get; set; }
        public int is_end { get; set; }
        public string[] language { get; set; }
        public int latest_episode { get; set; }
        public int mark_type { get; set; }
        public object[] movie_type { get; set; }
        public object[] music_type { get; set; }
        public object[] new_type { get; set; }
        public int online_count { get; set; }
        public int pay_mark { get; set; }
        public string[] place { get; set; }
        public int play_count { get; set; }
        public object[] property { get; set; }
        public string publish_date { get; set; }
        public string[] school { get; set; }
        public float score { get; set; }
        public object[] season { get; set; }
        public string[] specification { get; set; }
        public object[] sport { get; set; }
        public string[] style { get; set; }
        public string sub_title { get; set; }
        public int total_episode { get; set; }
        public string[] type { get; set; }
        public long update_time { get; set; }
        public string update_tip { get; set; }
        public string uploader_name { get; set; }
        public object[] version { get; set; }
        public object[] video_type { get; set; }
        public int year { get; set; }
    }
}
