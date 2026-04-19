using GreenSwampApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenSwampApp.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext dbContext)
    {
        var frogUser = GetOrCreateUser(
            dbContext,
            username: "frogmaster",
            email: "frogmaster@greenswamp.local",
            displayName: "Frog Master",
            bio: "I study swamp frogs and write about them.",
            avatarUrl: "https://i.pravatar.cc/150?img=12");

        var chessUser = GetOrCreateUser(
            dbContext,
            username: "chessbishop",
            email: "chessbishop@greenswamp.local",
            displayName: "Chess Bishop",
            bio: "I post chess tactics and tournament notes.",
            avatarUrl: "https://i.pravatar.cc/150?img=32");

        dbContext.SaveChanges();

        var tagFrogs = GetOrCreateTag(dbContext, "frogs");
        var tagSwamp = GetOrCreateTag(dbContext, "swamp");
        var tagChess = GetOrCreateTag(dbContext, "chess");
        var tagTactics = GetOrCreateTag(dbContext, "tactics");

        dbContext.SaveChanges();

        var seedItems = new List<(string Content, long UserId, DateTime CreatedAt, string MediaType, string MediaUrl, string[] Tags)>
        {
            (
                "Morning in the swamp: 12 tree frogs were active after the rain. #frogs #swamp",
                frogUser.UserId,
                DateTime.UtcNow.AddMinutes(-45),
                "none",
                string.Empty,
                ["frogs", "swamp"]
            ),
            (
                "Fun fact: many frogs can absorb water directly through their skin. #frogs",
                frogUser.UserId,
                DateTime.UtcNow.AddMinutes(-30),
                "none",
                string.Empty,
                ["frogs"]
            ),
            (
                "Puzzle of the day: look for a discovered attack in this middlegame setup. #chess #tactics",
                chessUser.UserId,
                DateTime.UtcNow.AddMinutes(-20),
                "none",
                string.Empty,
                ["chess", "tactics"]
            ),
            (
                "Rapid games improve intuition, classical games improve calculation. Both matter. #chess",
                chessUser.UserId,
                DateTime.UtcNow.AddMinutes(-10),
                "none",
                string.Empty,
                ["chess"]
            ),
            (
                "Found a bright green frog near the reeds today. Markings looked almost geometric. #frogs #swamp",
                frogUser.UserId,
                DateTime.UtcNow.AddMinutes(-8),
                "image",
                "https://images.unsplash.com/photo-1456926631375-92c8ce872def?auto=format&fit=crop&w=1200&q=80",
                ["frogs", "swamp"]
            ),
            (
                "Board snapshot from blitz training: knight outpost won the endgame. #chess #tactics",
                chessUser.UserId,
                DateTime.UtcNow.AddMinutes(-5),
                "image",
                "https://images.unsplash.com/photo-1528819622765-d6bcf132f793?auto=format&fit=crop&w=1200&q=80",
                ["chess"]
            )
        };

        var existingContents = dbContext.Posts
            .AsNoTracking()
            .Select(p => p.Content)
            .ToHashSet();

        var allTags = dbContext.Tags.ToDictionary(t => t.Name, t => t);
        var addedPosts = new List<(Post Post, string[] Tags)>();

        foreach (var item in seedItems)
        {
            if (existingContents.Contains(item.Content))
            {
                continue;
            }

            var post = new Post
            {
                UserId = item.UserId,
                Content = item.Content,
                MediaUrl = item.MediaUrl,
                MediaType = item.MediaType,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.CreatedAt
            };

            dbContext.Posts.Add(post);
            addedPosts.Add((post, item.Tags));
        }

        dbContext.SaveChanges();

        var postTags = new List<PostTag>();
        foreach (var added in addedPosts)
        {
            foreach (var tagName in added.Tags)
            {
                postTags.Add(new PostTag
                {
                    PostId = added.Post.PostId,
                    TagId = allTags[tagName].TagId
                });
            }
        }

        if (postTags.Count == 0)
        {
            return;
        }

        dbContext.PostTags.AddRange(postTags);
        dbContext.SaveChanges();
    }

    private static User GetOrCreateUser(
        ApplicationDbContext dbContext,
        string username,
        string email,
        string displayName,
        string bio,
        string avatarUrl)
    {
        var existing = dbContext.Users.FirstOrDefault(u => u.Username == username);
        if (existing != null)
        {
            existing.AvatarUrl = avatarUrl;
            existing.UpdatedAt = DateTime.UtcNow;
            return existing;
        }

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = "seed-password-hash",
            DisplayName = displayName,
            Bio = bio,
            AvatarUrl = avatarUrl,
            CoverImageUrl = string.Empty,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        return user;
    }

    private static Tag GetOrCreateTag(ApplicationDbContext dbContext, string tagName)
    {
        var normalized = tagName.Trim().ToLowerInvariant();
        var existing = dbContext.Tags.FirstOrDefault(t => t.Name == normalized);
        if (existing != null)
        {
            return existing;
        }

        var tag = new Tag
        {
            Name = normalized,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Tags.Add(tag);
        return tag;
    }
}
