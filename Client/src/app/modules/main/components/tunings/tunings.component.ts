import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { TuningDto } from 'src/app/models/dtos/tuning.dto';
import { AddTuningDialogComponent } from 'src/app/modules/dialog/components/tunings/add-tuning-dialog/add-tuning-dialog.component';
import { StringService } from 'src/app/shared/services/string.service';
import { TuningService } from 'src/app/shared/services/tuning.service';

@Component({
  selector: 'app-tunings',
  templateUrl: './tunings.component.html',
  styleUrls: ['./tunings.component.scss']
})
export class TuningsComponent implements OnInit {

  checkboxChecked: boolean = false;
  allTunings: TuningDto[] = [];
  tuningsWithTabs: TuningDto[] = [];
  tunings: TuningDto[] = [];
  formControl: FormControl;

  constructor(
    private readonly dialog: MatDialog,
    private readonly tuningService: TuningService,
    private readonly stringService: StringService) {
      this.formControl = new FormControl('');
    }

  ngOnInit(): void {
    this.tuningService.fetchTunings()
      .subscribe(response => {
        this.allTunings = response.sort(this.stringService.compareNames);
        this.tuningsWithTabs = this.allTunings.filter(x => x.tabs.length > 0);

        this.tunings = JSON.parse(JSON.stringify(this.tuningsWithTabs));
      })

    this.formControl.valueChanges.subscribe(value => {
      this.tunings = this.checkboxChecked
        ? this.allTunings.filter(x => x.name.toLocaleLowerCase().includes(value.toLocaleLowerCase()))
        : this.tuningsWithTabs.filter(x => x.name.toLocaleLowerCase().includes(value.toLocaleLowerCase()));
    });
  }

  addTuning() {
    const dialogRef = this.dialog.open(AddTuningDialogComponent, {
      width: '400px',
    });

    dialogRef
      .afterClosed()
      .subscribe((result: FormGroup) => {
        if (result !== undefined) {
          this.tuningService.addTuning({
            name: result.controls["name"].value,
            tabs: [],
            id: '00000000-0000-0000-0000-000000000000',
            stringNumber: result.controls["stringNumber"].value,
            strings: result.controls["strings"].value,
            instrument: result.controls["strings"].value,
          })
            .subscribe(response => {
              this.tunings.push(response.object);
              this.tunings.sort(this.stringService.compareNames);
              
              this.allTunings.push(JSON.parse(JSON.stringify(response.object)));
              this.allTunings.sort(this.stringService.compareNames);
            })
        }
      })
  }

  onCheckboxChange(event: any) {
    if (!event.checked) {
      this.tunings = this.tunings.filter(x => x.tabs.length > 0)
    } else {
      this.tunings = JSON.parse(JSON.stringify(this.allTunings))
    }

    this.checkboxChecked = event.checked;
  }

  rerenderTunings(event: any) {
    const allTuningsIndex = this.allTunings.indexOf(this.allTunings.filter(x => x.id === event.tuning.id)[0]);
    const allTuningsWithTabs = this.tuningsWithTabs.indexOf(this.tuningsWithTabs.filter(x => x.id === event.tuning.id)[0]);
    const allTunings = this.tunings.indexOf(this.tunings.filter(x => x.id === event.tuning.id)[0]);

    this.allTunings[allTuningsIndex] = event.tuning;
    this.tuningsWithTabs[allTuningsWithTabs] = event.tuning;
    this.tunings[allTunings] = event.tuning;

    this.allTunings = this.allTunings.sort(this.stringService.compareNames);
    this.tuningsWithTabs = this.tuningsWithTabs.sort(this.stringService.compareNames);
    this.tunings = this.tunings.sort(this.stringService.compareNames);
  }

}
