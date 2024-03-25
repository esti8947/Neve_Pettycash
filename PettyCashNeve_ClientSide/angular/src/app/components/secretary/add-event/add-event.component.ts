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
import { ConfirmationService } from 'primeng/api';
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
  selectedEvent:Event|undefined;
  editedEventName:string|undefined;

  constructor(
    private translateService:TranslateService,
    private formBuilder: FormBuilder,
    private eventService: EventService,
    private eventCategoryService: EventCategoryService,
    private router:Router,
    private customMessageService:CustomMessageService,
    private monthlyCashRegisterService: MontlyCashRegisterService,
    private confirmationService:ConfirmationService,
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
    const monthlyCashRegister = this.monthlyCashRegisterService.getCurrentMothlyRegister();
    const currentMonth = monthlyCashRegister.monthlyCashRegisterMonth;
    const currentYear = monthlyCashRegister.monthlyCashRegisterYear;
    const newEvent = {
      eventId: 0,
      eventName: this.formGroup.value.eventName,
      eventMonth: currentMonth,
      eventYear: currentYear,
      eventCategoryId: this.formGroup.value.selectedEventCategory.eventCategoryId,
      updatedBy: "string"
    };

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
    this.router.navigate(['/navbar/add-expense'], 
    { queryParams: { selectedEvent:  JSON.stringify(selectedEvent)} });
  }

  deleteEvent(event:MouseEvent, selectedEvent:any){
    const eventIdToDelete = selectedEvent.eventId;
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: this.translateService.instant('messages.deleteExpenseConfirmation'),
      icon: 'pi pi-info-circle',
      acceptButtonStyleClass: ' p-button-sm',
      accept: () => {
        this.eventService.deleteEvent(eventIdToDelete).subscribe(
          (data) => {
            console.log('event is deleted', data);
            this.customMessageService.showSuccessMessage(this.translateService.instant('messages.eventDeleted'));
            this.existingEvents = this.existingEvents.filter((val) => val.eventId !== eventIdToDelete);
          },
          (error) =>{
            this.customMessageService.showErrorMessage(this.translateService.instant('messages.eventUpdateError'));
          }
        )
      },
    })
  }

  editEvent(event:Event){
    this.selectedEvent = event;
    this.editedEventName = event.eventName;
  }

  saveEventChanges(event:Event){
    if(!this.editedEventName || !event) return;

    event.eventName = this.editedEventName;
    this.eventService.updateEvent(event).subscribe(
      (data)=>{
        this.selectedEvent = undefined;
        console.log('Event updated successfully:', data.data);
        this.customMessageService.showSuccessMessage('Event updated successfully');
        const index = this.existingEvents.findIndex((e) => e.eventId === event.eventId);
        if (index !== -1) {
          this.existingEvents[index] = event;
        }
      },
      (error) => {
        console.error('An error occurred while updating the event:', error);
        this.customMessageService.showErrorMessage('An error occurred while updating the event');
      }
    );
    this.editedEventName = '';
  }
}


