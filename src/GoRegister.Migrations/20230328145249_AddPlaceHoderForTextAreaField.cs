using Microsoft.EntityFrameworkCore.Migrations;

namespace GoRegister.Migrations
{
    public partial class AddPlaceHoderForTextAreaField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "EmailLayout",
            //    keyColumn: "Id",
            //    keyValue: 1);

            //migrationBuilder.DeleteData(
            //    table: "EmailTemplate",
            //    keyColumn: "Id",
            //    keyValue: 1);

            //migrationBuilder.DeleteData(
            //    table: "Email",
            //    keyColumn: "Id",
            //    keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "TextField_Placeholder",
                table: "Field",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextField_Placeholder",
                table: "Field");

        //    migrationBuilder.InsertData(
        //        table: "Email",
        //        columns: new[] { "Id", "Bcc", "Cc", "Description", "EmailLayoutId", "EmailType", "IsEnabled", "Name", "ProjectId", "Subject" },
        //        values: new object[] { 1, "", "", "Base MRF Email", null, 6, true, "Base MRF Email", 1, "MRF Confirmation Email" });

        //    migrationBuilder.InsertData(
        //        table: "EmailLayout",
        //        columns: new[] { "Id", "Html", "Name", "ProjectId" },
        //        values: new object[] { 1, "<p><code style=\" - webkit - text - stroke - width:0px; background - color:rgb(248, 249, 252); box - sizing:border - box; color: rgb(232, 62, 140); font - family:SFMono - Regular, Menlo, Monaco, Consolas, &quot; Liberation Mono&quot;, &quot; Courier New&quot;, monospace; font - size:11.2px; font - style:normal; font - variant - caps:normal; font - variant - ligatures:normal; font - weight:400; letter - spacing:normal; orphans: 2; overflow - wrap:break-word; text - align:left; text - decoration - color:initial; text - decoration - style:initial; text - decoration - thickness:initial; text - indent:0px; text - transform:none; white - space:normal; widows: 2; word - spacing:0px; \">{{email_content}}</code><span style=\"background - color:rgb(248, 249, 252); color: rgb(133, 135, 150); \"><span>&nbsp;</span></span></p>", "Base MRF Email", 1 });

        //    migrationBuilder.InsertData(
        //        table: "EmailTemplate",
        //        columns: new[] { "Id", "BodyHtml", "BodyText", "EmailId", "HasTextBody", "IsDefault", "ProjectId" },
        //        values: new object[] { 1, "<p>&lt;html&gt;<br>&lt;body style=\"font - size:13px; margin: 0px auto; background - color: #ced9e8a8;\"&gt;<br>&lt;font face=\"Helvetica, Arial, sans-serif\"&gt;<br>&lt;div style=\"width:600px; margin:0px auto;\"&gt;<br>&lt;div style=\"height: 80px;background-color: white;padding: 20px 2% 10px 2%;width: 96%;border-bottom: 5px solid #001C6F;\"&gt;<br>&lt;img src=\"https://goregister-mrf-development-fec291ca88f5e699.s3.eu-west-1.amazonaws.com/Project/Upload/meetingsevent.png\" /&gt;<br>&lt;/div&gt;<br>&lt;div style=\"height:226px;background-color:white;width:100%;\"&gt;<br>&lt;img src=\"https://goregister-mrf-development-fec291ca88f5e699.s3.eu-west-1.amazonaws.com/Project/Upload/emailbanner.png\" /&gt;<br>&lt;/div&gt;<br>&lt;div style=\"min-height:200px;background-color:white;padding:20px 2% 10px 2%;width:96%;position:relative;\"&gt;<br>&lt;p&gt;Dear [UserName],&lt;p&gt;<br>&lt;p&gt;Please find your MRF Form Information below&lt;/p&gt;<br>&lt;div style=\"width:100%; min-height:110px;\"&gt;<br>&lt;table cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%;font-size: 13px;\"&gt;<br>&lt;tr style=\"height: 50px;text-align: center;\"&gt;<br>&lt;td style=\"width:35%;border:1px solid #776f7f;font-weight:bold;font-size: 13px;font-family: Helvetica, Arial, sans-serif;text-align:center;\"&gt;<br>Field<br>&lt;/td&gt;<br>&lt;td style=\"width:65%;border:1px solid #776f7f;font-weight:bold;font-size: 13px;font-family: Helvetica, Arial, sans-serif;text-align:center;\"&gt;<br>Value<br>&lt;/td &gt;<br>&lt;/tr&gt;<br>[MRFFormInformation]<br>&lt;/table&gt;<br>&lt;/div&gt;<br>&lt;/div&gt;<br>&lt;div style=\"height: 30px;background-color: white;width: 96%;padding: 5px 2% 5px 2%;\"&gt;<br>&lt;img src=\"https://goregister-mrf-development-fec291ca88f5e699.s3.eu-west-1.amazonaws.com/Project/Upload/business.png\" /&gt;<br>&lt;/div&gt;<br>&lt;/div&gt;<br>&lt;div style=\"width:100%;background-color:#00175A;margin:0px;height: 310px;\"&gt;<br>&lt;div style=\"width: 600px;margin: 0px auto;color: white;font-size: 11px;\"&gt;<br>&lt;div style=\"width: 37%;height: 280px;margin-top: 8px;padding-left:2%;padding-right: 2%;float:left;border-bottom: 1px solid #FFFFFF;border-right: 1px solid #FFFFFF;line-height: 1.6;padding-top: 5px;\"&gt;<br>&lt;p&gt;This is an automated message. Please do not reply to this e-mail.&lt;/p&gt;<br>&lt;p&gt;This email was sent to [UserEmailId] from [FromEmailId]&lt;/p&gt;<br>&lt;/div&gt;<br>&lt;div style=\"width: 54%;height: 280px;margin-top: 8px;padding-left:2%;padding-right: 2%;float: left;border-bottom: 1px solid #FFFFFF;line-height: 1.6;padding-top: 5px;\"&gt;<br>&lt;p&gt;American Express Global Business Travel's services use personal information as described in the GBT Global Privacy Statement. &lt;/p&gt;<br>&lt;p&gt;American Express Global Business Travel (GBT) is a joint venture that is not wholly owned by American Express Company or any of its subsidiaries (American Express). “American Express Global Business Travel”, \"American Express,” and the American Express logo are trademarks of American Express and are used under limited license.&lt;/p&gt;&nbsp;<br>&lt;p&gt;This document contains unpublished, confidential, and proprietary information of American Express &nbsp;Global Business Travel (GBT). No disclosure or use of any portion of these materials may be made without &nbsp;the express written consent of GBT. &lt;/p&gt;<br>&lt;p&gt;©️2022 GBT Travel Services UK Limited &lt;/p&gt;<br>&lt;/div&gt;<br>&lt;/div&gt;<br>&lt;/div&gt;<br>&lt;/body&gt;<br>&lt;/html&gt;</p>", "", 1, false, true, 1 });
        }
    }
}
