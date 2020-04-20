using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PMSDemo.DataExporting.Excel.NPOI;
using PMSDemo.Deliverables.Dtos.Exporting;
using PMSDemo.Dto;
using PMSDemo.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PMSDemo.Deliverables.Exporting
{
    public class DeliverableExcelExporter : NpoiExcelExporterBase, IDeliverableExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DeliverableExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(MdaDeliverableExportDto mdaDeliverables)
        {
            return CreateExcelPackage(
                "Test.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Deliverable");
                    //Samp
                    var deliverable = mdaDeliverables.deliverables[0];

                    //column width
                    sheet.SetColumnWidth(0, 10000);
                    sheet.SetColumnWidth(1, 10000);
                    sheet.SetColumnWidth(2, 5000);
                    sheet.SetColumnWidth(3, 5000);
                    sheet.SetColumnWidth(4, 5000);
                    sheet.SetColumnWidth(5, 5000);
                    sheet.SetColumnWidth(6, 5000);
                    sheet.SetColumnWidth(7, 5000);
                    //sheet.SetColumnWidth(8, 5000);

                    //Create cell border stlye
                    var cellStyleBorder = excelPackage.CreateCellStyle();
                    cellStyleBorder.BorderBottom = BorderStyle.Thin;
                    cellStyleBorder.BorderLeft = BorderStyle.Thin;
                    cellStyleBorder.BorderRight = BorderStyle.Thin;
                    cellStyleBorder.BorderTop = BorderStyle.Thin;
                    cellStyleBorder.Alignment = HorizontalAlignment.Center;
                    cellStyleBorder.VerticalAlignment = VerticalAlignment.Center;
                    cellStyleBorder.WrapText = true;

                    #region Main title
                    //Create Main Title Style
                    var mainTitleStyle = excelPackage.CreateCellStyle();
                    var mainTitleFont = excelPackage.CreateFont();
                    mainTitleStyle.CloneStyleFrom(cellStyleBorder);
                    mainTitleStyle.FillPattern = FillPattern.SolidForeground;
                    ((XSSFCellStyle)mainTitleStyle).SetFillForegroundColor(new XSSFColor(new byte[] { 55, 86, 35 }));
                    mainTitleFont.Color = IndexedColors.White.Index;
                    mainTitleFont.FontHeightInPoints = 14;
                    mainTitleFont.IsBold = true;
                    mainTitleStyle.SetFont(mainTitleFont);

                    //Create Main Title cell
                    var mainTitleRow = sheet.CreateRow(0);
                    mainTitleRow.HeightInPoints = 40;
                    mainTitleRow.CreateCell(0, CellType.String).SetCellValue("PERFORMANCE REPORT FOR THE SEVENTEEN (17) DELIVERABLES OF THE MINISTRY FINANCE BUDGET AND NATIONAL PLANNING AS AT: " + DateTime.Now.ToString());
                    mainTitleRow.Cells[0].CellStyle = mainTitleStyle;
                    for (int i = 1; i < 8; i++)
                    {
                        var cell = mainTitleRow.CreateCell(i);
                        cell.CellStyle = mainTitleStyle;
                    }
                    var mainTitleRange = new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 7);
                    sheet.AddMergedRegion(mainTitleRange);
                    #endregion

                    #region Mda title
                    //Create MDA Title style
                    var mdaTitleStyle = excelPackage.CreateCellStyle();
                    var mdaTitleFont = excelPackage.CreateFont();
                    mdaTitleStyle.CloneStyleFrom(cellStyleBorder);
                    mdaTitleStyle.FillPattern = FillPattern.SolidForeground;
                    ((XSSFCellStyle)mdaTitleStyle).SetFillForegroundColor(new XSSFColor(new byte[] { 218, 218, 218 }));
                    mdaTitleFont.FontHeightInPoints = 14;
                    mdaTitleFont.IsBold = true;
                    mdaTitleStyle.SetFont(mdaTitleFont);

                    //Create MDA Title cell
                    var mdaTitleRow = sheet.CreateRow(1);
                    //mdaTitleRow.HeightInPoints = 40;
                    mdaTitleRow.CreateCell(0, CellType.String).SetCellValue("MDA: " + mdaDeliverables.MdaName);
                    mdaTitleRow.Cells[0].CellStyle = mdaTitleStyle;
                    for (int i = 1; i < 8; i++)
                    {
                        var cell = mdaTitleRow.CreateCell(i);
                        cell.CellStyle = mdaTitleStyle;
                    }
                    var mdaTitleRange = new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 7);
                    sheet.AddMergedRegion(mdaTitleRange);

                    #endregion

                    #region Priority area title
                    //Create Priority area Title style
                    var areaTitleStyle = excelPackage.CreateCellStyle();
                    var areaTitleFont = excelPackage.CreateFont();
                    areaTitleStyle.CloneStyleFrom(cellStyleBorder);
                    areaTitleStyle.FillPattern = FillPattern.SolidForeground;
                    areaTitleStyle.FillForegroundColor = HSSFColor.Grey40Percent.Index;
                    areaTitleFont.Color = IndexedColors.White.Index;
                    areaTitleFont.FontHeightInPoints = 12;
                    areaTitleFont.IsBold = true;
                    areaTitleStyle.SetFont(areaTitleFont);

                    //Create priority area Title cell
                    var areaTitleRow = sheet.CreateRow(2);
                    //areaTitleRow.HeightInPoints = 40;
                    areaTitleRow.CreateCell(0, CellType.String).SetCellValue("PART 1:   GOVERNMENT PRIORITY AREA: " + deliverable.Deliverable.PriorityAreaName);
                    areaTitleRow.Cells[0].CellStyle = areaTitleStyle;
                    for (int i = 1; i < 8; i++)
                    {
                        var cell = areaTitleRow.CreateCell(i);
                        cell.CellStyle = areaTitleStyle;
                    }
                    var areaTitleRange = new NPOI.SS.Util.CellRangeAddress(2, 2, 0, 7);
                    sheet.AddMergedRegion(areaTitleRange);

                    #endregion


                    #region Part1 
                    #region header
                    var infoHeaderStyle = excelPackage.CreateCellStyle();
                    var infoHeaderFont = excelPackage.CreateFont();
                    infoHeaderStyle.CloneStyleFrom(cellStyleBorder);
                    infoHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    infoHeaderStyle.FillForegroundColor = HSSFColor.White.Index;
                    infoHeaderFont.Color = IndexedColors.Black.Index;
                    infoHeaderFont.FontHeightInPoints = 12;
                    infoHeaderFont.IsBold = true;
                    infoHeaderStyle.SetFont(infoHeaderFont);

                    var indicatorHeaderRow = sheet.CreateRow(3);
                    indicatorHeaderRow.CreateCell(0, CellType.String).SetCellValue("Deliverable");
                    indicatorHeaderRow.CreateCell(1, CellType.String).SetCellValue("Achievement of Results/Performance Delivery Indicators");
                    indicatorHeaderRow.CreateCell(2, CellType.String).SetCellValue("Baseline");
                    indicatorHeaderRow.CreateCell(3, CellType.String).SetCellValue("Target Year");
                    indicatorHeaderRow.CreateCell(4, CellType.String).SetCellValue("Target");
                    indicatorHeaderRow.CreateCell(5, CellType.String).SetCellValue("Actual Performance");
                    indicatorHeaderRow.CreateCell(6, CellType.String).SetCellValue("Unit of Measure");
                    indicatorHeaderRow.CreateCell(7, CellType.String).SetCellValue("Means of Verification");

                    for (int i = 0; i < 8; i++)
                    {
                        indicatorHeaderRow.Cells[i].CellStyle = infoHeaderStyle;
                    }
                    #endregion

                    #region body
                    var infoBodyStyle = excelPackage.CreateCellStyle();
                    var infoBodyFont = excelPackage.CreateFont();
                    infoBodyStyle.CloneStyleFrom(cellStyleBorder);
                    infoBodyFont.FontHeightInPoints = 12;
                    infoBodyStyle.SetFont(infoBodyFont);

                    var rowIndex = 4;
                    foreach (var item in deliverable.Indicators)
                    {
                        var indicatorBodyRow = sheet.CreateRow(rowIndex);
                        indicatorBodyRow.CreateCell(0, CellType.String).SetCellValue(item.DeliverableName);
                        indicatorBodyRow.CreateCell(1, CellType.String).SetCellValue(item.PerformanceIndicator.Name);
                        indicatorBodyRow.CreateCell(2, CellType.String).SetCellValue(item.PerformanceIndicator.BaselineComment);
                        indicatorBodyRow.CreateCell(3, CellType.String).SetCellValue(item.PerformanceIndicator.BaselineComment);
                        indicatorBodyRow.CreateCell(4, CellType.String).SetCellValue(item.PerformanceIndicator.BaselineComment);
                        indicatorBodyRow.CreateCell(5, CellType.String).SetCellValue(item.PerformanceIndicator.BaselineComment);
                        indicatorBodyRow.CreateCell(6, CellType.String).SetCellValue(item.PerformanceIndicator.BaselineComment);
                        indicatorBodyRow.CreateCell(7, CellType.String).SetCellValue(item.PerformanceIndicator.DataType.ToString());
                        rowIndex++;
                        for (int i = 0; i < 8; i++)
                        {
                            indicatorBodyRow.Cells[i].CellStyle = infoBodyStyle;
                        }
                    }
                    var indicatorBodyRange = new NPOI.SS.Util.CellRangeAddress(4, rowIndex - 1, 0, 0);
                    sheet.AddMergedRegion(indicatorBodyRange);

                    #endregion body
                    #endregion Part1

                    #region Part2 title

                    //Create Part 2 Title cell
                    var part2TitleRow = sheet.CreateRow(rowIndex); 
                    //areaTitleRow.HeightInPoints = 40;
                    part2TitleRow.CreateCell(0, CellType.String).SetCellValue("PART 2: Activities leading to Achievement of the Deliverable");
                    part2TitleRow.Cells[0].CellStyle = areaTitleStyle;
                    for (int i = 1; i < 8; i++)
                    {
                        var cell = part2TitleRow.CreateCell(i);
                        cell.CellStyle = areaTitleStyle;
                    }
                    var part2TitleRange = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 7); rowIndex++;
                    sheet.AddMergedRegion(part2TitleRange);

                    #endregion

                    #region Part2 
                    #region header
                    var activityHeaderRow = sheet.CreateRow(rowIndex); rowIndex++;
                    activityHeaderRow.CreateCell(0, CellType.String).SetCellValue("Activity Name");
                    activityHeaderRow.CreateCell(1, CellType.String).SetCellValue("Milestone Achieved");
                    activityHeaderRow.CreateCell(2, CellType.String).SetCellValue("Status of Completion (%)");
                    activityHeaderRow.CreateCell(3, CellType.String).SetCellValue("Planned Start Date");
                    activityHeaderRow.CreateCell(4, CellType.String).SetCellValue("Actual Start Date");
                    activityHeaderRow.CreateCell(5, CellType.String).SetCellValue("Planned Completion Date");
                    activityHeaderRow.CreateCell(6, CellType.String).SetCellValue("Actual Completion Date");
                    activityHeaderRow.CreateCell(7, CellType.String).SetCellValue("Additional Information");

                    for (int i = 0; i < 8; i++)
                    {
                        activityHeaderRow.Cells[i].CellStyle = infoHeaderStyle;
                    }
                    #endregion

                    #region body
                    foreach (var item in deliverable.Activities)
                    {
                        var activityBodyRow = sheet.CreateRow(rowIndex);
                        activityBodyRow.CreateCell(0, CellType.String).SetCellValue(item.PerformanceActivity.Name);
                        activityBodyRow.CreateCell(1, CellType.String).SetCellValue(item.PerformanceActivity.MilestoneAchieved);
                        activityBodyRow.CreateCell(2, CellType.String).SetCellValue(item.PerformanceActivity.CompletionLevel.ToString());
                        activityBodyRow.CreateCell(3, CellType.String).SetCellValue(DateFormat(item.PerformanceActivity.PlannedStartDate));
                        activityBodyRow.CreateCell(4, CellType.String).SetCellValue(DateFormat(item.PerformanceActivity.PlannedCompletionDate));
                        activityBodyRow.CreateCell(5, CellType.String).SetCellValue(DateFormat(item.PerformanceActivity.ActualStartDate));
                        activityBodyRow.CreateCell(6, CellType.String).SetCellValue(DateFormat(item.PerformanceActivity.ActualCompletionDate));
                        activityBodyRow.CreateCell(7, CellType.String).SetCellValue(StripHTML(item.PerformanceActivity.Note));

                        rowIndex++;
                        for (int i = 0; i < 8; i++)
                        {
                            activityBodyRow.Cells[i].CellStyle = infoBodyStyle;
                        }
                    }

                    #endregion body
                    #endregion Part2

                    #region Part3 title

                    //Create Part 3 Title cell
                    var part3TitleRow = sheet.CreateRow(rowIndex);
                    part3TitleRow.CreateCell(0, CellType.String).SetCellValue("PART 3:  Remarks/Challenges/Recommendations/Comments");
                    part3TitleRow.Cells[0].CellStyle = areaTitleStyle;
                    for (int i = 1; i < 8; i++)
                    {
                        var cell = part3TitleRow.CreateCell(i);
                        cell.CellStyle = areaTitleStyle;
                    }
                    var part3TitleRange = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 7); rowIndex++;
                    sheet.AddMergedRegion(part3TitleRange);

                    #endregion

                    #region Part3
                    #region header
                    var othersHeaderRow = sheet.CreateRow(rowIndex);
                    var othersHeaderCellA = othersHeaderRow.CreateCell(0, CellType.String);
                    othersHeaderCellA.SetCellValue("Remark/comment");
                    othersHeaderCellA.CellStyle = infoHeaderStyle;
                    var othersHeaderCellB = othersHeaderRow.CreateCell(1);
                    othersHeaderCellB.CellStyle = infoHeaderStyle;
                    var commentsHeaderRange = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 1);
                    sheet.AddMergedRegion(commentsHeaderRange);

                    var othersHeaderCellD = othersHeaderRow.CreateCell(2, CellType.String);
                    othersHeaderCellD.SetCellValue("Challenges");
                    othersHeaderCellD.CellStyle = infoHeaderStyle;
                    var othersHeaderCellE = othersHeaderRow.CreateCell(3);
                    othersHeaderCellE.CellStyle = infoHeaderStyle;
                    var othersHeaderCellF = othersHeaderRow.CreateCell(4);
                    othersHeaderCellF.CellStyle = infoHeaderStyle;
                    var challengesHeaderRange = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 2, 4);
                    sheet.AddMergedRegion(challengesHeaderRange);

                    var othersHeaderCellG = othersHeaderRow.CreateCell(5, CellType.String);
                    othersHeaderCellG.SetCellValue("Recommendations");
                    othersHeaderCellG.CellStyle = infoHeaderStyle;
                    var othersHeaderCellH = othersHeaderRow.CreateCell(6);
                    othersHeaderCellH.CellStyle = infoHeaderStyle;
                    var othersHeaderCellC = othersHeaderRow.CreateCell(7);
                    othersHeaderCellC.CellStyle = infoHeaderStyle;
                    var recommendationsHeaderRange = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 5, 7);
                    sheet.AddMergedRegion(recommendationsHeaderRange);

                    rowIndex++;
                    #endregion

                    #region body
                    foreach (var item in deliverable.Reviews)
                    {
                        var othersBodyRow = sheet.CreateRow(rowIndex);
                        var othersBodyCellA = othersBodyRow.CreateCell(0, CellType.String);
                        othersBodyCellA.SetCellValue(StripHTML(item.Review.ReviewComment));
                        othersBodyCellA.CellStyle = infoBodyStyle;
                        var othersBodyCellB = othersBodyRow.CreateCell(1);
                        othersBodyCellB.CellStyle = infoBodyStyle;
                        var commentBodyRange = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, 1);
                        sheet.AddMergedRegion(commentBodyRange);

                        var othersBodyCellD = othersBodyRow.CreateCell(2, CellType.String);
                        othersBodyCellD.SetCellValue(StripHTML(item.Review.Challenges));
                        othersBodyCellD.CellStyle = infoBodyStyle;
                        var othersBodyCellE = othersBodyRow.CreateCell(3);
                        othersBodyCellE.CellStyle = infoBodyStyle;
                        var othersBodyCellF = othersBodyRow.CreateCell(4);
                        othersBodyCellF.CellStyle = infoBodyStyle;
                        var challengesBodyRange = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 2, 4);
                        sheet.AddMergedRegion(challengesBodyRange);

                        var othersBodyCellG = othersBodyRow.CreateCell(5, CellType.String);
                        othersBodyCellG.SetCellValue(StripHTML(item.Review.Recommendation));
                        othersBodyCellG.CellStyle = infoBodyStyle;
                        var othersBodyCellH = othersBodyRow.CreateCell(6);
                        othersBodyCellH.CellStyle = infoBodyStyle;
                        var othersBodyCellC = othersBodyRow.CreateCell(7);
                        othersBodyCellC.CellStyle = infoBodyStyle;
                        var recommendationsBodyRange = new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 5, 7);
                        sheet.AddMergedRegion(recommendationsBodyRange);

                        rowIndex++;
                    }

                    #endregion body
                    #endregion
                });
        }


        public string StripHTML(string input)
        {
            if (input == null)
                return "";

            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public string DateFormat(DateTime? input)
        {
            var d = DateTime.Parse("01/01/0001");
            if (input == null || input == d)
            {
                return "";
            }

            //DateTime d = DateTime.Parse(input);
            return ((DateTime)input).ToString("dd MMM yyyy");

        }
    }
}
