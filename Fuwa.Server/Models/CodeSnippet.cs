using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fuwa.Models
{
    public class CodeSnippet
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string? AuthorTag { get; set; }

        [ForeignKey(nameof(AuthorTag))]
        public User? Author { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Code { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        [Required]
        public CodeLanguage CodeLanguage { get; set; }

        [InverseProperty("LikedSnippets")]
        public ICollection<User> LikedBy { get; set; } = new List<User>();
    }

    public enum CodeLanguage
    {
        C,
        Cpp,
        Csharp,
        HTML,
        CSS,
        SCSS,
        Python,
        Ruby,
        Java,
        Javascript,
        Rust,
        Zig,
        EmacsLisp,
        CommonLisp,
        TypeScript,
        PHP,
        JupyterNotebook,
        Go,
        Haskell,
        OCaml,
        Fsharp,
        Assembly,
        Kotlin,
        Swift,
        Dart,
        Scala,
        Racket,
        Erlang,
        Lua,
        Elixir,
        Angular,
        React,
        Svelte,
        Vue,
        Plaintext,
        Markdown,
        Unknown
    }
}
