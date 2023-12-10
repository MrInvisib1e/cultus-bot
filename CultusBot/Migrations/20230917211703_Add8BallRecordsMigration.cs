﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CultusBot.Migrations
{
    /// <inheritdoc />
    public partial class Add8BallRecordsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO [dbo].[ChatMessages](Text, Type, IsActive) VALUES
(N'Магія каже, що так.', 100, 1),
(N'Так.', 100, 1),
(N'Неначе під Фелікс-Феліцісем.', 100, 1),
(N'Можна вважати, що так.', 100, 1),
(N'Фіренце бачить лише позитивні варіанти.', 100, 1),
(N'Зорі пророкують успіх.', 100, 1),
(N'Оооо, давно пророцтво не було таким чітким. Так.', 100, 1),
(N'Септима Вектор вирахувала шанси, як позитивні.', 100, 1),
(N'Сіра Пані бачила таке уже. Тоді все закінчилось успішно.', 100, 1),
(N'Люди скажуть ні, але я кажу так.', 100, 1),
(N'У небі недавно пролітала Китайська метеорка. Це позитивна прикмета.', 100, 1),
(N'Якщо кавова гуща не бреше, тоді так', 100, 1),
(N'Апареціум на пергаменті проявив відповідь - ""Так""', 100, 1),
(N'Моя відповідь так', 100, 1),
(N'Так, точно так.', 100, 1),
(N'Ти можеш розраховувати на це.', 100, 1),
(N'Флаґрате вивело у повітрі ""Так"".', 100, 1),
(N'У статті Віщуна писали, що так.', 100, 1),
(N'Трелоні побачила у кулі - ""Так""', 100, 1),
(N'Невже не очевидно, що так?', 100, 1),
(N'Марс сьогодні яскравий, спробуйте пізніше.', 100, 1),
(N'Сфінкс заплутався у варіантах.', 100, 1),
(N'Трелоні не бачить нічого сьогодні, бо трошки хильнула вина.', 100, 1),
(N'Спитайтесь трохи пізніше.', 100, 1),
(N'Півз відволік мене, спитайтеся ще раз.', 100, 1),
(N'Троль розбив кулю пророцтв. Спробуйте завтра.', 100, 1),
(N'Невимовники не можуть нічого сказати.', 100, 1),
(N'Упс, якісь мародери наклали Сіленціо.', 100, 1),
(N'Ніби нагодували асфоделем. Нічого не пам''ятаю.', 100, 1),
(N'""Медові руці"" були зачинені сьогодні. Не достатньо енергії для гадань.', 100, 1),
(N'Ні.', 100, 1),
(N'Русалії вночі співали - це негативна прикмета.', 100, 1),
(N'Чорний кнізл дорогу перебіг. Точно ні.', 100, 1),
(N'Ритуал каже, що точно ні.', 100, 1),
(N'Моя відповідь ні.', 100, 1),
(N'Леприкони дали своє ""Так"", через деякий час воно щезло і стало ""НІ"".', 100, 1),
(N'Апареціум на пергаменті проявив відповідь - ""Ні"".', 100, 1),
(N'Флаґрате вивело у повітрі ""Ні"".', 100, 1),
(N'У статті Віщуна писали, що ні.', 100, 1),
(N'Невже не очевидно, що відповідь буде ""Ні""?', 100, 1)
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
