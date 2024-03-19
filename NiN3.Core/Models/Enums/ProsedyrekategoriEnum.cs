using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NiN3.Core.Models.Enums
{
    public enum ProsedyrekategoriEnum
    {
        //[Description("")]
        //Default,
        [Description("Normal variasjonsbredde. Variasjon i artssammensetning ikke betinget av strukturerende artsgruppe. Lite endret system.")]
        A,
        [Description("Normal variasjonsbredde. Variasjon i artssammensetning betinget av strukturerende artsgruppe. Lite endret system.")]
        B,
        [Description("Spesiell variasjonsbredde. Variasjon i artssammensetning ikke betinget av strukturerende artsgruppe. Lite endret system. Preget av miljøstress.")]
        C,
        [Description("Spesiell variasjonsbredde. Variasjon i artssammensetning ikke betinget av strukturerende artsgruppe. Lite endret system. Preget av aktiv regulerende forstyrrelse .")]
        D,
        [Description("Spesiell variasjonsbredde. Variasjon i artssammensetning betinget av strukturerende artsgruppe. Lite endret system. Preget av aktiv destabiliserende forstyrrelse.")]
        E,
        [Description("Spesiell variasjonsbredde. Variasjon i artssammensetning betinget av strukturerende artsgruppe. Lite endret system. Preget av aktiv regulerende forstyrrelse .")]
        F,
        [Description("Spesiell variasjonsbredde. Ny mark eller bunn . Klart endret system. Preget av historisk forstyrrelse.")]
        G,
        [Description("Spesiell variasjonsbredde. Variasjon i artssammensetning betinget av bortfall av strukturerende artsgruppe. Klart endret system. Uten preg av hevd.")]
        H,
        [Description("Spesiell variasjonsbredde. Variasjon i artssammensetning betinget av strukturerende artsgruppe. Klart endret system. Uten preg av hevd.")]
        I,
        [Description("Spesiell variasjonsbredde. Klart endret system. Hevdpreget. Uten jordbruksproduksjon.")]
        J,
        [Description("Spesiell variasjonsbredde. Klart endret system. Hevdpreget. Semi-naturlig system. Med historisk dybde.")]
        K,
        [Description("Spesiell variasjonsbredde. Klart endret system. Hevdpreget. Semi-naturlig system. Uten historisk dybde.")]
        L,
        [Description("Spesiell variasjonsbredde. Ny mark eller bunn . Sterkt endret system. Uten preg av hevd.")]
        M,
        [Description("Spesiell variasjonsbredde. Sterkt endret system. Hevdpreget. Uten jordbruksproduksjon.")]
        N,
        [Description("Spesiell variasjonsbredde. Sterkt endret system. Hevdpreget. Jordbruksmark.")]
        O
    }
}
