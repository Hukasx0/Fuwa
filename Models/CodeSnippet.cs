using System.ComponentModel.DataAnnotations;

namespace Fuwa.Models
{
    public class CodeSnippet
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string? AuthorTag { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Code { get; set; }
        [Required]
        public string? CreatedDate { get; set; }
        [Required]
        public string? LastModifiedDate { get; set; }
        [Required]
        public CodeLanguage CodeLanguage { get; set;  }
        public ICollection<User> LikedBy { get; set; } = null!;
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
