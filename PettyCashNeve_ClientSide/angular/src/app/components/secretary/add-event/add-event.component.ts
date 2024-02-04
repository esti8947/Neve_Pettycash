import { Component, OnInit } from '@angular/core';
import { EventCategory } from 'src/app/models/eventCategory';
import { Event } from 'src/app/models/event';
import { EventCategoryService } from 'src/app/services/event-service/event-category.service';
import { EventService } from 'src/app/services/event-service/event.service';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CustomMessageService } from 'src/app/services/customMessage-service/custom-message.service';
import { MontlyCashRegisterService } from 'src/app/services/montlyCashRegister-service/montly-cash-register.service';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'add-event',
  templateUrl: './add-event.component.html',
  styleUrls: ['./add-event.component.scss'],
})
export class AddEventComponent implements OnInit {
  eventCategories: EventCategory[] = [];
  existingEvents: Event[] = [];
  formGroup!: FormGroup;
  selectedExistingEvent: Event | undefined;
  validForm: boolean = true;
  initiallySelectedEvent: Event | null = null;
  isMonthlyCashRegister:boolean = false;

  constructor(
    private translateService:TranslateService,
    private formBuilder: FormBuilder,
    private eventService: EventService,
    private eventCategoryService: EventCategoryService,
    private router:Router,
    private customMessageService:CustomMessageService,
    private monthlyCashRegisterService: MontlyCashRegisterService,
    ) {}

  ngOnInit(): void {
    this.loadExistingEvents();
    this.loadEventCategories();
    this.initializeForm();
    this.isMonthlyRegister();
  }

  isMonthlyRegister(){
    const isMonthlyRegisterSet = this.monthlyCashRegisterService.getCurrentMothlyRegister();
    if(isMonthlyRegisterSet)
    this.isMonthlyCashRegister = true;
  }

  loadEventCategories() {
    this.eventCategoryService.getEventsCategories().subscribe(
      (data) => {
        this.eventCategories = data.data;
        console.log("eventcategories",this.eventCategories);
      },
      (error) => {
        console.error('An error occurred:', error);
      },
    );
  }

  loadExistingEvents() {
    this.eventService.getEventsByUser().subscribe(
      (data) => {
        this.existingEvents = data.data;
        console.log(this.existingEvents);
      },
      (error) => {
        console.error('An error occurred:', error);        
      },
    );
  }

  getOptionLabel(){
    return this.translateService.currentLang === 'en-US'? 'eventCategoryName':'eventCategoryNameHeb';
  }

  initializeForm() {
    this.formGroup = this.formBuilder.group({
      selectedEventCategory: new FormControl<any | null>(null,Validators.required),
      eventName: new FormControl<string>('', Validators.required),
    });
  }

  addEvent() {
  this.validForm = !this.formGroup.invalid;
  if(this.validForm && this.isMonthlyCashRegister){
    const currentMonth = this.monthlyCashRegisterService.getCurrentMothlyRegister().monthlyCashRegisterMonth;
    const newEvent = {
      eventId: 0,
      eventName: this.formGroup.value.eventName,
      eventMonth: currentMonth,
      eventCategoryId: this.formGroup.value.selectedEventCategory.eventCategoryId,
      updatedBy: "string"
    };

    console.log("New Event: ", newEvent);
    this.eventService.addNewEvent(newEvent).subscribe(
      (response) => {
        console.log('Event added successfully:', response);
        this.customMessageService.showSuccessMessage('Event added successfully')
        this.formGroup.reset();
        this.loadExistingEvents();
      },
      (error) => {
        console.error('An error occurred while adding the event:', error);
        this.customMessageService.showErrorMessage('An error occurred while adding the event')
      }
    )}
    else{
      if(!this.isMonthlyCashRegister)
      this.customMessageService.showErrorMessage('Cannot add events before setting up a monthly cash register.')
    }
  };

  addExpenseToSelectedEvent(selectedEvent:Event){
    console.log(selectedEvent);
    this.router.navigate(['/navbar/add-expense'], 
    { queryParams: { selectedEvent:  JSON.stringify(selectedEvent)} });
  }
}


